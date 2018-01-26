using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FamilyBook.Entity
{ 
    /// <summary>
    /// Parameter
    /// Database.AddInParameter(new DbCommand(), this.Name, this.Type, this.Value);
    /// </summary>
    public class Parameter
    {
        public Parameter(string name)
        {
            this.name = name;
        }

        public Parameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public Parameter(string name, DbType type, object value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }

        /// <summary>
        /// Database field name or parameter name
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Database field type of database or parameter type of database
        /// </summary>
        private DbType type;
        public DbType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Database field value or parameter value
        /// </summary>
        private object value;
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    } 
}
