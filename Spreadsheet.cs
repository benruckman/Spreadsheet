using SpreadsheetUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Dictionary that takes in a key, and maps to a cell Object
        /// </summary>
        private Dictionary<String, Cell> sheet;
        /// <summary>
        /// Dependency Graph for dependencys in the spreadsheet
        /// </summary>
        private DependencyGraph dg;

        // ADDED FOR PS5
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        public Spreadsheet() : base(x => true, x => x, "default")
        {
            sheet = new Dictionary<String, Cell>();
            dg = new DependencyGraph();
            Changed = false;
        }

        // ADDED FOR PS5
        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            sheet = new Dictionary<String, Cell>();
            dg = new DependencyGraph();
            Changed = false;
        }

        public Spreadsheet(String filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            sheet = new Dictionary<String, Cell>();
            dg = new DependencyGraph();
            Changed = false;
            try
            {
                ReadFromXml(filePath, version);
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Directory not found");
            }
        }

        private void ReadFromXml(string filePath, string version)
        {
            bool nothing = true;
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                String name = null;
                try
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {

                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    /*if (!reader["version"].Equals(version))
                                        throw new SpreadsheetReadWriteException("Input version doesn't equal the version read from Xml Doc");*/
                                    break;

                                case "cell":
                                    break; //Nothing to do with just the cell

                                case "name":
                                    name = reader.ReadElementContentAsString();
                                    if (reader.Name.Equals("contents"))
                                    {
                                        SetContentsOfCell(name, reader.ReadElementContentAsString());
                                        nothing = false;
                                        name = null;
                                    }
                                    break;
                                case "contents":
                                    if (name == null)
                                        throw new SpreadsheetReadWriteException("Contents tag came before name");
                                    if (reader.Value.Equals(""))
                                        SetContentsOfCell(name, reader.ReadElementContentAsString());
                                    else
                                        SetContentsOfCell(name, reader.Value);
                                    nothing = false;
                                    name = null;
                                    break;
                            }
                        }
                    }
                }
                catch (XmlException)
                {
                    throw new SpreadsheetReadWriteException("Xml Exception");
                }
                if (nothing)
                    throw new SpreadsheetReadWriteException("Nothing was inside of the spreadsheet Xml");
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null)
                throw new InvalidNameException();
            if (!IsValidName(name))
                throw new InvalidNameException();
            if (!IsValid(name))
                throw new InvalidNameException();
            if (!sheet.ContainsKey(Normalize(name)))
            {
                return "";
            }
            return sheet[Normalize(name)].GetContents();
        }

        private bool IsValidName(string c)
        {
            //Must have at least one letter
            //Must have at least one digit, following all letters

            c = c.Trim();

            bool foundLetter = false;
            bool foundDigit = false;
            int i;
            for (i = 0; i < c.Length; i++)
            {
                //Check if s[i] is a letter, and stop if its not a letter
                if (Char.IsLetter(c[i]))
                {
                    foundLetter = true;
                }
                else
                    break;
            }

            for (; i < c.Length; i++)
            {
                //Check if s[i] is a digit, and return false if its not a digit
                if (Char.IsDigit(c[i]))
                {
                    foundDigit = true;
                }
                else
                    return false;
            }

            return foundLetter && foundDigit;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return sheet.Keys;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, double number)
        {
            //Adds number to a cell, and adds cell to sheet. 
            RemoveFromSheetAndAdd(name, number, "double");

            //Making the returned list
            return MakeReturnedList(name);
        }

        /// <summary>
        /// Removes the Key from the sheet, if it exists in the sheet, and then adds the key with a new cell using the passed in object to the sheet
        /// </summary>
        /// <param name="name">
        /// The key/name passed into the set methods
        /// </param>
        /// <param name="obj">
        /// The object passed into the set methods (double number, string text, or Formula formula)
        /// </param>
        private void RemoveFromSheetAndAdd(string name, Object obj, string type)
        {
            if (sheet.Remove(name)) //If sheet doesn't have name, then doesnt do anything
            {
                if (dg.HasDependents(name)) //If this cell had dependents, removes them all
                {
                    foreach (string dependent in dg.GetDependents(name))
                    {
                        dg.RemoveDependency(name, dependent);
                    }
                }
            }
            sheet.Add(name, new Cell(obj, Lookup, type));
            /*            if (obj.GetType().Equals(new FormulaError().GetType()))
                            throw new CircularException();*/
        }

        private double Lookup(string name)
        {
            if (sheet.TryGetValue(name, out Cell cell))
            {
                //change to is statement
                if (!cell.GetValue().GetType().Equals(new FormulaError().GetType()))
                {
                    if (!cell.Type().Equals("string"))
                        return (double)cell.GetValue();
                }
            }
            throw new ArgumentException("Didn't find variable");
        }



        /// <summary>
        /// Helper method to make the list returned by the set methods
        /// </summary>
        /// <param name="name">
        /// Key/Name from the set methods
        /// </param>
        /// <returns>
        /// The list to return in the set methods
        /// </returns>
        private IList<string> MakeReturnedList(string name)
        {
            ISet<string> a = new HashSet<string>();
            a.Add(name);
            bool inLoop = false;
            IEnumerable<string> cells;
            foreach (string s in dg.GetDependees(name))
            {
                inLoop = true;
                a.Add(s);
            }
            if (inLoop)
                cells = GetCellsToRecalculate(name);
            else
                cells = GetCellsToRecalculate(a);

            IList<string> list = new List<string>();

            foreach (string s in cells)
            {
                list.Add(s);
            }

            return list;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, string text)
        {

            //Adds text to a cell, and adds cell to sheet. 
            RemoveFromSheetAndAdd(name, text, "string");


            //Making the returned list
            return MakeReturnedList(name);
        }
        /// <summary>
        /// Validates the name and input. Makes sure they are not null before or after being normalized, 
        /// and makes sure they pass validation of IsValid, and isValidName before and after being normalized
        /// </summary>
        /// <param name="name">
        /// Input name
        /// </param>
        /// <param name="input">
        /// Input (formula, double, or text)
        /// </param>
        private void ValidateNameAndInput(string name, Object input)
        {
            if (input == null)
                throw new ArgumentNullException();

            if (name == null)
                throw new InvalidNameException();
            if (!IsValidName(name))
                throw new InvalidNameException();
            if (!IsValid(name))
                throw new InvalidNameException();

            if (Normalize(name) == null)
                throw new InvalidNameException();
            if (!IsValidName(Normalize(name)))
                throw new InvalidNameException();
            if (!IsValid(Normalize(name)))
                throw new InvalidNameException();
        }


        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            if (CircularExceptionCaused(name, formula))
                throw new CircularException();

            //Adds number to a cell, and adds cell to sheet. 
            RemoveFromSheetAndAdd(name, formula, "formula");

            IEnumerable<string> vars = formula.GetVariables(); //Gets all variables from the formula

            foreach (string var in vars) //For all variables in the formula, adds a dependency, so that this cell is dependent on variables in the formula
            {
                dg.AddDependency(name, var);
            }

            //Making the returned list
            return MakeReturnedList(name);
        }

        private bool CircularExceptionCaused(string name, Formula formula)
        {
            foreach (string var in formula.GetVariables())
            {
                foreach (string dependent in dg.GetDependents(var))
                {
                    if (dependent.Equals(name))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dg.GetDependees(name);
        }

        // ADDED FOR PS5
        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    reader.ReadToFollowing("spreadsheet");
                    return reader.GetAttribute("version");
                }
            }
            catch (FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File not found");
            }
        }

        // ADDED FOR PS5
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            try
            {
                Changed = false;

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    foreach (string key in GetNamesOfAllNonemptyCells())
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", key);
                        if (sheet[key].Type().Equals("formula"))
                            writer.WriteElementString("contents", "=" + GetCellContents(key).ToString());
                        else
                            writer.WriteElementString("contents", GetCellContents(key).ToString());
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File Not Found");
            }
        }

        // ADDED FOR PS5
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if (name == null)
                throw new InvalidNameException();
            if (!IsValidName(name))
                throw new InvalidNameException();
            if (!IsValid(name))
                throw new InvalidNameException();
            try
            {
                return sheet[name].GetValue();
            }
            catch (KeyNotFoundException)
            {
                return "";
            }
        }

        // ADDED FOR PS5
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown,
        ///       and no change is made to the spreadsheet.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a list consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            ValidateNameAndInput(name, content);

            Changed = true;
            IList<string> ret;
            string type = "";

            if (double.TryParse(content, out double result))
            {
                ret = SetCellContents(Normalize(name), result);
                type = "double";
            }
            else if (content.StartsWith("="))
            {
                ret = SetCellContents(Normalize(name), new Formula(content.Substring(1), Normalize, IsValid));
                type = "formula";
            }
            else
            {
                ret = SetCellContents(Normalize(name), content);
                type = "string";
            }
            RecalculateValues(ret, type);
            return ret;
        }

        private void RecalculateValues(IList<string> ret, string type)
        {
            foreach (string key in ret)
            {
                Object contents = GetCellContents(key);
                type = sheet[key].Type();
                sheet[key] = new Cell(contents, Lookup, type);
            }
        }

        private class Cell
        {
            /// <summary>
            /// Stores what the cell has inside of it
            /// </summary>
            private Object item;

            private Object value;

            private string type;

            /// <summary>
            /// Creates a new cell
            /// </summary>
            /// <param name="contained">
            /// What is inside of the cell
            /// </param>
            public Cell(Object contained, Func<string, double> lookup, string type)
            {
                this.type = type;
                item = contained;

                if (type.Equals("double") || type.Equals("string"))
                    value = contained;
                else
                {
                    Formula f1 = new Formula(contained.ToString());
                    value = f1.Evaluate(lookup);
                }
            }

            /// <summary>
            /// Gets whats inside of the cell
            /// </summary>
            /// <returns>
            /// Whats in the cell
            /// </returns>
            public Object GetContents()
            {
                return item;
            }

            public Object GetValue()
            {
                return value;
            }

            internal string Type()
            {
                return type;
            }
        }
    }
}
