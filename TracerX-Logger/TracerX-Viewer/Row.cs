using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using TracerX.Properties;
using TracerX.Forms;

namespace TracerX.Viewer {
    // Represents one row of data in the viewer.  Several contiguous rows can map to
    // a single Record object if the Record contains newlines and has been expanded.
    internal class Row {
        public Row(Record record, int rowIndex, int recordLine) {
            Index = rowIndex;
            Init(record, recordLine);
        }

        public void Init(Record record, int recordLine) {
            Rec = record;
            Line = (ushort)recordLine;
        }

        // The Record object whose data is shown on this row.
        public Record Rec;

        // The index of this object in the _rows array.
        public int Index;

        // If the text in Record contains embedded newlines and it has been split/expanded
        // into multiple lines, this is the index of the line to display on this row.
        public ushort Line = 0;

        public bool IsBookmarked {
            get { return Rec.IsBookmarked[Line]; }
            set { Rec.IsBookmarked[Line] = value; }
        }

        // Used by Find and CallStack.
        // Returns non-indented, non-truncated text from the Text column.
        public override string ToString() {
            return Rec.GetLine(Line, ' ', 0, false);
        }

        // Returns indented, non-truncated text.
        // Used for copying text to the clipboard.
        public string GetFullIndentedText() {
            return Rec.GetLine(Line, Settings.Default.IndentChar, Settings.Default.IndentAmount, false);
        }

        // Array used to initialize the subitems of each ListViewItem generated by MakeItem.
        private static string[] _fields = new string[9];
        private const int _bookmarkIndex = 0;
        private const int _plusIndex = 1;
        private const int _minusIndex = 3;
        private const int _downIndex = 5;
        private const int _upIndex = 7;
        private const int _sublineIndex = 10;
        private const int _lastSublineIndex = 12;

        // Make a ListViewItem from a Row object.
        public ViewItem MakeItem(Row previousRow) {
            SetFields(previousRow);

            ViewItem item = new ViewItem(_fields, ImageIndex);

            item.Row = this;
            item.Tag = this;

            if (Settings.Default.ColoringEnabled) {
                switch (ColorRulesDialog.CurrentTab) {
                    case ColorRulesDialog.ColorTab.Custom:
                        ColoringRule rule = MatchColoringRule();
                        if (rule != null) {
                            item.BackColor = item.BColor = rule.BackColor;
                            item.ForeColor = item.FColor = rule.TextColor;
                            item.UseItemStyleForSubItems = true;
                        }
                        break;

                    case ColorRulesDialog.ColorTab.TraceLevels:
                        if (ColorRulesDialog.TraceLevelColors[Rec.Level].Enabled) {
                            item.BackColor = item.BColor = ColorRulesDialog.TraceLevelColors[Rec.Level].BackColor;
                            item.ForeColor = item.FColor = ColorRulesDialog.TraceLevelColors[Rec.Level].ForeColor;
                            item.UseItemStyleForSubItems = true;
                        }
                        break;
                    case ColorRulesDialog.ColorTab.ThreadIDs:
                        if (Rec.Thread.Colors != null) {
                            item.BackColor = item.BColor = Rec.Thread.Colors.BackColor;
                            item.ForeColor = item.FColor = Rec.Thread.Colors.ForeColor;
                            item.UseItemStyleForSubItems = true;
                        }
                        break;
                    case ColorRulesDialog.ColorTab.Sessions:
                        if (Rec.Session.Colors != null) {
                            item.BackColor = item.BColor = Rec.Session.Colors.BackColor;
                            item.ForeColor = item.FColor = Rec.Session.Colors.ForeColor;
                            item.UseItemStyleForSubItems = true;
                        }
                        break;
                    case ColorRulesDialog.ColorTab.ThreadNames:
                        if (Rec.ThreadName.Colors != null) {
                            item.BackColor = item.BColor = Rec.ThreadName.Colors.BackColor;
                            item.ForeColor = item.FColor = Rec.ThreadName.Colors.ForeColor;
                            item.UseItemStyleForSubItems = true;
                        }
                        break;
                    case ColorRulesDialog.ColorTab.Loggers:
                        if (Rec.Logger.Colors != null) {
                            item.BackColor = item.BColor = Rec.Logger.Colors.BackColor;
                            item.ForeColor = item.FColor = Rec.Logger.Colors.ForeColor;
                            item.UseItemStyleForSubItems = true;
                        }
                        break;
                    case ColorRulesDialog.ColorTab.Methods:
                        if (Rec.MethodName.Colors != null) {
                            item.BackColor = item.BColor = Rec.MethodName.Colors.BackColor;
                            item.ForeColor = item.FColor = Rec.MethodName.Colors.ForeColor;
                            item.UseItemStyleForSubItems = true;
                        } else if (ColorRulesDialog.ColorCalledMethods) {
                            Record caller = Rec.Caller;

                            while (caller != null) {
                                if (caller.MethodName.Colors != null) {
                                    item.BackColor = item.BColor = caller.MethodName.Colors.BackColor;
                                    item.ForeColor = item.FColor = caller.MethodName.Colors.ForeColor;
                                    item.UseItemStyleForSubItems = true;
                                    break;
                                }

                                caller = caller.Caller;
                            }

                        }

                        break;
                }
            }

            return item;
        }

        // Tests this Row's data against each ColoringRule.  Returns the first rule
        // that matches.  Returns null if no match.
        private ColoringRule MatchColoringRule() {
            if (Settings.Default.ColoringRules != null) {
                MainForm mainForm = MainForm.TheMainForm;
                string text = null;
                string level = null;
                string logger = null;
                string threadName = null;
                string method = null;

                foreach (ColoringRule rule in Settings.Default.ColoringRules) {
                    if (rule.Enabled) {
                        // Test each field specified by rule.Fields.
                        // MustMatch and MustNotMatch cannot both be null, but either can be.
                        // Only one field needs to match MustMatch (if present), but all fields
                        // must not match MustNotMatch (if present).
                        bool mustMatchFound = rule.MustMatch == null;

                        if (rule.Fields == ColoringFields.All || (rule.Fields & ColoringFields.Level) == ColoringFields.Level) {
                            // Acquire the level value if we don't already have it.
                            if (level == null) {
                                // Get it from _fields if possible.
                                if (mainForm.headerLevel.Index != -1) level = _fields[mainForm.headerLevel.Index];
                                else level = Enum.GetName(typeof(TraceLevel), Rec.Level);
                            }

                            if (rule.MustNotMatch == null) {
                                if (rule.MustMatch.Matches(level)) return rule;
                            } else {
                                // If any field matches rule.MustNotMatch, continue to the next rule.
                                if (rule.MustNotMatch.Matches(level)) continue;
                                else if (!mustMatchFound) mustMatchFound = rule.MustMatch.Matches(level);
                            }
                        }

                        if (rule.Fields == ColoringFields.All || (rule.Fields & ColoringFields.Logger) == ColoringFields.Logger) {
                            if (logger == null) logger = Rec.Logger.Name;

                            if (rule.MustNotMatch == null) {
                                if (rule.MustMatch.Matches(logger)) return rule;
                            } else {
                                if (rule.MustNotMatch.Matches(logger)) continue;
                                else if (!mustMatchFound) mustMatchFound = rule.MustMatch.Matches(logger);
                            }
                        }

                        if (rule.Fields == ColoringFields.All || (rule.Fields & ColoringFields.Method) == ColoringFields.Method) {
                            if (method == null) method = Rec.MethodName.Name;

                            if (rule.MustNotMatch == null) {
                                if (rule.MustMatch.Matches(method)) return rule;
                            } else {
                                if (rule.MustNotMatch.Matches(method)) continue;
                                else if (!mustMatchFound) mustMatchFound = rule.MustMatch.Matches(method);
                            }
                        }

                        if (rule.Fields == ColoringFields.All || (rule.Fields & ColoringFields.Text) == ColoringFields.Text) {
                            // Use the non-indented, non-truncated text.
                            if (text == null) text = Rec.GetLine(Line, ' ', 0, false);

                            if (rule.MustNotMatch == null) {
                                if (rule.MustMatch.Matches(text)) return rule;
                            } else {
                                if (rule.MustNotMatch.Matches(text)) continue;
                                else if (!mustMatchFound) mustMatchFound = rule.MustMatch.Matches(text);
                            }
                        }

                        if (rule.Fields == ColoringFields.All || (rule.Fields & ColoringFields.ThreadName) == ColoringFields.ThreadName) {
                            if (threadName == null) threadName = Rec.ThreadName.Name;

                            if (rule.MustNotMatch == null) {
                                if (rule.MustMatch.Matches(threadName)) return rule;
                            } else {
                                if (rule.MustNotMatch.Matches(threadName)) continue;
                                else if (!mustMatchFound) mustMatchFound = rule.MustMatch.Matches(threadName);
                            }
                        }

                        if (mustMatchFound) return rule;
                    } // if rule.Enabled
                } // foreach rule
            }

            return null;
        }

        private void SetFields(Row previousRow) {
            int columnCount = MainForm.TheMainForm.TheListView.Columns.Count;
            MainForm mainForm = MainForm.TheMainForm;

            if (_fields.Length != columnCount) _fields = new string[columnCount];

            // If a column has been removed from the ListView, 
            // its ListView property will be null.

            if (mainForm.headerText.ListView != null) {
                _fields[mainForm.headerText.Index] = Rec.GetLine(Line, Settings.Default.IndentChar, Settings.Default.IndentAmount, true);
            }

            if (mainForm.headerSession.ListView != null) {
                _fields[mainForm.headerSession.Index] = Rec.Session.Name;
            }

            if (mainForm.headerLine.ListView != null) {
                _fields[mainForm.headerLine.Index] = Rec.GetRecordNum(Line);
            }

            if (mainForm.headerLevel.ListView != null) {
                _fields[mainForm.headerLevel.Index] = Enum.GetName(typeof(TraceLevel), Rec.Level);
            }

            if (mainForm.headerLogger.ListView != null) {
                _fields[mainForm.headerLogger.Index] = Rec.Logger.Name;
            }

            if (mainForm.headerThreadId.ListView != null) {
                _fields[mainForm.headerThreadId.Index] = Rec.ThreadId.ToString();
            }

            if (mainForm.headerThreadName.ListView != null) {
                _fields[mainForm.headerThreadName.Index] = Rec.ThreadName.Name;
            }

            if (mainForm.headerTime.ListView != null) {
                if (previousRow == null || previousRow.Rec.Time != this.Rec.Time || Settings.Default.DuplicateTimes) {
                    if (Settings.Default.RelativeTime) {
                        _fields[mainForm.headerTime.Index] = Program.FormatTimeSpan(Rec.Time - MainForm.ZeroTime);
                    } else if (Settings.Default.UseCustomTimeFormat) {
                        _fields[mainForm.headerTime.Index] = Rec.Time.ToLocalTime().ToString(Settings.Default.CustomTimeFormat);
                    } else {
                        _fields[mainForm.headerTime.Index] = Rec.Time.ToLocalTime().ToString(@"MM/dd/yy HH:mm:ss.fff");
                    }
                } else {
                    _fields[mainForm.headerTime.Index] = string.Empty;
                }
            }

            if (mainForm.headerMethod.ListView != null) {
                _fields[mainForm.headerMethod.Index] = Rec.MethodName.Name;
            }
        }

        // Returns the image index to use for this row.
        public int ImageIndex {
            get {
                int ret = -1;

                if (Rec.IsEntry) {
                    if (Rec.IsCollapsed) {
                        ret = _plusIndex;
                    } else {
                        ret = _minusIndex;
                    }
                } else if (Rec.HasNewlines) {
                    if (Index == Rec.FirstRowIndex) {
                        // This is the first visible line.
                        if (Rec.IsCollapsed) {
                            ret = _downIndex;
                        } else {
                            ret = _upIndex;
                        }
                    } else if (Line == Rec.Lines.Length - 1) {
                        ret = _lastSublineIndex;
                    } else {
                        ret = _sublineIndex;
                    }
                }

                if (IsBookmarked) {
                    ret += 1;
                }

                return ret;
            }
        }

        public void ShowFullText() {
            FullText ft = new FullText(Rec.GetTextForWindow(Line), true);
            ft.Show(MainForm.TheMainForm);
        }
    }
}
