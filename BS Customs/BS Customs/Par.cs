using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace BIMtrovert.BS_Customs
{
    class Par
    {
        public Parameter ID { get; set; }
        public string Name { get; set; }

        public bool Equals(Par other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            int hashParName = Name == null ? 0 : Name.GetHashCode();

            return hashParName;
        }
    }
}
