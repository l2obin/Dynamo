﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSNodeServices;
using RevitServices.Transactions;

namespace DSRevitNodes.Elements
{
    /// <summary>
    /// A Revit Family
    /// </summary>
    [RegisterForTrace]
    public class DSFamily : AbstractElement
    {
        #region Internal properties

        internal Autodesk.Revit.DB.Family InternalFamily
        {
            get; private set;
        }

        #endregion

        #region Private constructors

        internal DSFamily(Autodesk.Revit.DB.Family family)
        {
            InternalSetFamily(family);
        }

        #endregion

        #region private mutators

        /// <summary>
        /// Set the internal Revit representation and update the ElementId and UniqueId
        /// </summary>
        /// <param name="family"></param>
        private void InternalSetFamily(Autodesk.Revit.DB.Family family)
        {
            this.InternalFamily = family;
            this.InternalElementId = family.Id;
            this.InternalUniqueId = family.UniqueId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Obtain the FamilySymbols from this Family
        /// </summary>
        public DSFamilySymbol[] Symbols
        {
            get
            {
                return InternalFamily.Symbols
                    .Cast<Autodesk.Revit.DB.FamilySymbol>()
                    .Select(x => new DSFamilySymbol(x))
                    .ToArray();
            }
        }

        #endregion

        #region Static constructors

        /// <summary>
        /// Obtain a Family from the current document given it's name
        /// </summary>
        /// <param name="name">The name of the family in the current document</param>
        /// <returns></returns>
        public static DSFamily ByName(string name)
        {
            TransactionManager.GetInstance().EnsureInTransaction(Document);

            // look up the loaded family
            var fec = new Autodesk.Revit.DB.FilteredElementCollector(Document);
            fec.OfClass(typeof(Autodesk.Revit.DB.Family));

            // obtain the family symbol with the provided name
            var families = fec.Cast<Autodesk.Revit.DB.Family>();

            var family = families.FirstOrDefault(x => x.Name == name);

            if (family == null)
            {
                throw new Exception("A FamilySymbol with the specified name does not exist in the document");
            }

            TransactionManager.GetInstance().TransactionTaskDone();

            return new DSFamily(family);
        }

        #endregion

        #region ToString override

        public override string ToString()
        {
            return String.Format("Family: {0}", InternalFamily.Name);
        }

        #endregion
    }
}