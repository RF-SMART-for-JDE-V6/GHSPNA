using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using ICS.Database;
using ICS.Database.TableObjects.JDE;
using ICS.Utilities;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Utilities.Printing
{
    /// <summary>
    /// Used to generate many base print variables, storing them in a build in DataTable
    /// </summary>
    public class GenPrintDataBase : IDisposable
    {
        #region Enums

        #region OverwriteMode

        /// <summary>
        /// An enumerator to specify the type of Overwrite Mode when setting print variables
        /// </summary>
        protected enum OverwriteMode
        {
            /// <summary>
            /// No Overwrite Mode specified
            /// </summary>
            None,

            /// <summary>
            /// Overwrite the value for the variable in the Current Row
            /// </summary>
            Current,

            /// <summary>
            /// Overwrite the value for the variable in the rows currently present
            /// </summary>
            All
        }

        #endregion

        #endregion

        #region Class Level Variables

        protected DAL _DataAccess;

        protected Logging _Log;

        private DataTable _PrintDataTable;

        private int _CurrentPartsIndex;

        protected Dictionary<string, double> _ConversionFactors;

        #region Tables

        protected CustomJDETables.F00092 _SupplementalDataTable;

        protected CustomJDETables.F0101 _AddressBookMasterTable;

        protected CustomJDETables.F0116 _AddressByDateTable;

        protected CustomJDETables.F4006 _OrderAddressInfoTable;

        protected CustomJDETables.F5547001 _CustomerPlantTable;

        protected F41001 _BranchPlantConstantsTable;

        protected F4101 _ItemMasterTable;

        protected F4102 _ItemBranchTable;

        protected F4108 _LotMasterTable;

        protected F41001 _WarehouseConstantsTable;

        protected F41002 _UnitOfMeasureConversionTable1;

        protected F41003 _UnitOfMeasureConversionTable2;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new GenPrintDataBase
        /// </summary>
        /// <param name="dataAccess">Data Access Layer</param>
        public GenPrintDataBase(DAL dataAccess)
        {
            this._DataAccess = dataAccess;
            this._Log = new Logging(_DataAccess.CurrentEnvironment, LogType.System, _DataAccess.System.Log.ID);
            this._ConversionFactors = new Dictionary<string, double>();

            ResetAll();

            this.IsDisposed = false;
        }

        #endregion

        #region Properties

        #region Tables

        #region SupplementalDataTable (F00092)

        /// <summary>
        /// Get the Supplemental Data Table (F00092)
        /// </summary>
        protected virtual CustomJDETables.F00092 SupplementalDataTable
        {
            get { return (_SupplementalDataTable ?? (_SupplementalDataTable = new CustomJDETables.F00092(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region AddressBookMasterTable (F0101)

        /// <summary>
        /// Get the Address Book Master Table (F0101)
        /// </summary>
        protected virtual CustomJDETables.F0101 AddressBookMasterTable
        {
            get { return (_AddressBookMasterTable ?? (_AddressBookMasterTable = new CustomJDETables.F0101(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region AddressByDateTable (F0116)

        /// <summary>
        /// Get the Address By Date Table (F0116)
        /// </summary>
        protected virtual CustomJDETables.F0116 AddressByDateTable
        {
            get { return (_AddressByDateTable ?? (_AddressByDateTable = new CustomJDETables.F0116(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region OrderAddressInfoTable (F4006)

        /// <summary>
        /// Get the Order Address Info Table (F4006)
        /// </summary>
        protected virtual CustomJDETables.F4006 OrderAddressInfoTable
        {
            get { return (_OrderAddressInfoTable ?? (_OrderAddressInfoTable = new CustomJDETables.F4006(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region CustomerPlantTable (F5547001)

        protected virtual CustomJDETables.F5547001 CustomerPlantTable
        {
            get { return (_CustomerPlantTable ?? (_CustomerPlantTable = new CustomJDETables.F5547001(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region BranchPlantConstantsTable

        protected virtual F41001 BranchPlantConstantsTable
        {
            get { return (_BranchPlantConstantsTable ?? (_BranchPlantConstantsTable = new F41001(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region ItemMasterTable (F4101)

        /// <summary>
        /// Get the Item Master Table (F4101)
        /// </summary>
        protected virtual F4101 ItemMasterTable
        {
            get { return (_ItemMasterTable ?? (_ItemMasterTable = new F4101(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region ItemBranchTable (F4102)

        /// <summary>
        /// Get the Item Branch Table (F4102)
        /// </summary>
        protected virtual F4102 ItemBranchTable
        {
            get { return (_ItemBranchTable ?? (_ItemBranchTable = new F4102(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region LotMasterTable (F4108)

        /// <summary>
        /// Get the Lot Master Table (F4108)
        /// </summary>
        protected virtual F4108 LotMasterTable
        {
            get { return (_LotMasterTable ?? (_LotMasterTable = new F4108(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region WarehouseConstantsTable (F41001)

        /// <summary>
        /// Get the Warehouse Constants Table (F41001)
        /// </summary>
        protected virtual F41001 WarehouseConstantsTable
        {
            get { return (_WarehouseConstantsTable ?? (_WarehouseConstantsTable = new F41001(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region UnitOfMeasureConversionTable1 (F41002)

        /// <summary>
        /// Get the Unit of Measure Conversion Table 1 (F41002)
        /// </summary>
        protected virtual F41002 UnitOfMeasureConversionTable1
        {
            get { return (_UnitOfMeasureConversionTable1 ?? (_UnitOfMeasureConversionTable1 = new F41002(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #region UnitOfMeasureConversionTable2 (F41003)

        /// <summary>
        /// Get the Unit of Measure Conversion Table 2 (F41003)
        /// </summary>
        protected virtual F41003 UnitOfMeasureConversionTable2
        {
            get { return (_UnitOfMeasureConversionTable2 ?? (_UnitOfMeasureConversionTable2 = new F41003(_DataAccess, "GenPrintDataBase_"))); }
        }

        #endregion

        #endregion

        #region PrintDataTable

        /// <summary>
        /// Get the PrintData DataTable
        /// </summary>
        protected DataTable PrintDataTable
        {
            get
            {
                if (_PrintDataTable == null)
                {
                    _PrintDataTable = new DataTable("PrintData");
                }

                return _PrintDataTable;
            }
        }

        #endregion

        #region CurrentDataRow

        /// <summary>
        /// Get the Current DataRow in the PrintData DataTable
        /// </summary>
        protected DataRow CurrentDataRow
        {
            get
            {
                if (PrintDataTable.Rows.Count <= 0)
                {
                    PrintDataTable.Rows.Add(PrintDataTable.NewRow());
                }

                return PrintDataTable.Rows[PrintDataTable.Rows.Count - 1];
            }
        }

        #endregion

        #region VariableNamePrefix

        /// <summary>
        /// Get or Set a prefix that will be appended to the beginning of each variable name
        /// </summary>
        public string VariableNamePrefix { get; set; }

        #endregion

        #region EnableMulitPartVariables

        /// <summary>
        /// Get or Set if the code can add multiple of the same variable name to a label
        /// appending a variable to the end of the name to allow for multiple instance of
        /// the same name on a label (useful for mixed or multi part like logic)
        /// </summary>
        public bool EnableMultiPartLabels { get; set; }

        #endregion

        #region MaxPartsPerLabel

        /// <summary>
        /// Get or Set the Maximum Parts Per Label. When EnableMultiPartLabels is true, specifies
        /// the maxiumum instances of the same instance of a variable name (with an appending number)
        /// can exist on the same label, before the counter is reset and a new label is started
        /// </summary>
        public int MaxPartsPerLabel { get; set; }

        #endregion

        #region IsDisposed

        /// <summary>
        /// Get if the Print Data Generator has been disposed of
        /// </summary>
        public bool IsDisposed { get; private set; }

        #endregion

        #endregion

        #region ResetAll

        /// <summary>
        /// Resets the PrintData DataTable back to a default state, with no data set yet, as well as all other
        /// dynamically adjustable variables
        /// </summary>
        public virtual void ResetAll()
        {
            PrintDataTable.Clear();
            PrintDataTable.Columns.Clear();

            EnableMultiPartLabels = false;
            MaxPartsPerLabel = -1;
            _CurrentPartsIndex = 1;
            _ConversionFactors.Clear();
        }

        #endregion

        #region IncrementPartsIndex

        /// <summary>
        /// If EnableMultiPartLabels is True, increments the index appended to the end of the variable's
        /// name. Should the index exceed the value specified in the MaxPartsPerLabel property, the index
        /// is reset, and a new row is added to the PrintData DataTable, this is if a valid positive value
        /// is set for the MaxPartsPerLabel property.
        /// </summary>
        public void IncrementPartsIndex()
        {
            IncrementPartsIndex(false);
        }

        /// <summary>
        /// If EnableMultiPartLabels is True, increments the index appended to the end of the variable's
        /// name. Should the index exceed the value specified in the MaxPartsPerLabel property, the index
        /// is reset, and a new row is added to the PrintData DataTable, this is if a valid positive value
        /// is set for the MaxPartsPerLabel property.
        /// </summary>
        /// <param name="copyData">If a new row is created, and this is True, all data from the previous row will be copied over</param>
        public void IncrementPartsIndex(bool copyData)
        {
            if (EnableMultiPartLabels)
            {
                _CurrentPartsIndex++;

                //If a valid MaxPartsPerLabel is valid, and the current index exceeds the value, then add a new row, and reset the counter
                if ((MaxPartsPerLabel > -1) && (_CurrentPartsIndex > MaxPartsPerLabel))
                {
                    _CurrentPartsIndex = 1;

                    //This allows for the ability to copy over data from the previous row, specifically items that
                    //are considered to be constants and such
                    if (copyData)
                    {
                        PrintDataTable.Rows.Add(CurrentDataRow.ItemArray);
                    }
                    else
                    {
                        PrintDataTable.Rows.Add(PrintDataTable.NewRow());
                    }
                }
            }
        }

        #endregion

        #region Generate Data

        #region GenerateShipFromAddress

        /// <summary>
        /// Generates the Ship From Address information
        /// </summary>
        /// <param name="addressNumber">Address Number (AN8)</param>
        public void GenerateShipFromAddress(double addressNumber, string branchPlant)
        {
            GenerateShipFromAddress(addressNumber, -1, -1, branchPlant);
        }

        /// <summary>
        /// Generates the Ship From Address information
        /// </summary>
        /// <param name="shipFromAddressNumber">Ship From Address Number (AN8), used for querying F0116 data</param>
        /// <param name="supplierAddressNumber">Supplier Address Number (AN8), used for querying F00092 data</param>
        /// <param name="shipToAddressNumber">Ship-To Address Number (SHAN)</param>
        public virtual void GenerateShipFromAddress(double shipFromAddressNumber, double supplierAddressNumber, double shipToAddressNumber, string branchPlant)
        {
            if (AddressByDateTable.RetrieveAddressInfo(shipFromAddressNumber))
            {
                AddPrintVariable("SRIAddress1", AddressByDateTable.ALADD1);
                AddPrintVariable("SRIAddress2", AddressByDateTable.ALADD2);
                AddPrintVariable("SRIAddress3", AddressByDateTable.ALADD3);
                AddPrintVariable("SRIAddress4", String.Format("{0}, {1} {2}", AddressByDateTable.ALCTY1, AddressByDateTable.ALADDS, AddressByDateTable.ALADDZ));
                AddPrintVariable("SRICity", AddressByDateTable.ALCTY1);
                AddPrintVariable("SRIState", AddressByDateTable.ALADDS);
                AddPrintVariable("SRIZip", AddressByDateTable.ALADDZ);
                AddPrintVariable("SRICountryCode", AddressByDateTable.ALCTR);
            }

            //If a valid Supplier Address Number and Ship-To Address Number are provided, the attempt to fetch (note we have to use a different AN8 value for the Supplier Code
            //in relation to the other Address information that is fetched above)
            //if ((supplierAddressNumber > -1) && (shipToAddressNumber > -1) &&  (SupplementalDataTable.RetrieveAddressCode("EDI", supplierAddressNumber, shipToAddressNumber)))
            if ((supplierAddressNumber > -1) && (shipToAddressNumber > -1) && (CustomerPlantTable.RetrieveAddressCode(branchPlant, supplierAddressNumber, shipToAddressNumber)))
            {
                AddPrintVariable("SRISupplierCode", CustomerPlantTable.ETSUPNM);
            }

            AddBlankVariables("SRIAddress1", "SRIAddress2", "SRIAddress3", "SRIAddress4", "SRICity", "SRIState", "SRIZip", "SRICountryCode", "SRISupplierCode");
        }

        #endregion

        #region GenerateShipToAddress

        /// <summary>
        /// Generates the Ship To Address information
        /// </summary>
        /// <param name="orderNumber">Order Number (DOCO)</param>
        /// <param name="orderType">Order Type (DCTO)</param>
        /// <param name="orderCompany">Order Company (KCOO)</param>
        /// <param name="shipToAddressNumber">Ship-To Address Number (SHAN)</param>
        public void GenerateShipToAddress(double orderNumber, string orderType, string orderCompany, double shipToAddressNumber, string branchPlant)
        {
            GenerateShipToAddress(orderNumber, orderType, orderCompany, shipToAddressNumber, -1, branchPlant);
        }

        /// <summary>
        /// Generates the Ship To Address information
        /// </summary>
        /// <param name="orderNumber">Order Number (DOCO)</param>
        /// <param name="orderType">Order Type (DCTO)</param>
        /// <param name="orderCompany">Order Company (KCOO)</param>
        /// <param name="shipToAddressNumber">Ship-To Address Number (SHAN)</param>
        /// <param name="addressNumber">Address Number (AN8)</param>
        public virtual void GenerateShipToAddress(double orderNumber, string orderType, string orderCompany, double shipToAddressNumber, double addressNumber, string branchPlant)
        {
            //if (OrderAddressInfoTable.RetrieveByOrderNumber(orderNumber, orderType, orderCompany))
            //{
            //    if (AddressBookMasterTable.FetchAddress(shipToAddressNumber))
            //    {
            //        AddPrintVariable("CustomerPlantName", AddressBookMasterTable.ABALPH);
            //    }

            //    AddPrintVariable("CustomerPlantMailingName", OrderAddressInfoTable.OAMLNM);
            //    AddPrintVariable("CustomerAddress1", OrderAddressInfoTable.OAADD1);
            //    AddPrintVariable("CustomerAddress2", OrderAddressInfoTable.OAADD2);
            //    AddPrintVariable("CustomerAddress3", OrderAddressInfoTable.OAADD3);
            //    AddPrintVariable("CustomerAddress4", String.Format("{0}, {1} {2}", OrderAddressInfoTable.OACTY1, OrderAddressInfoTable.OAADDS, OrderAddressInfoTable.OAADDZ));
            //    AddPrintVariable("CustomerCity", OrderAddressInfoTable.OACTY1);
            //    AddPrintVariable("CustomerState", OrderAddressInfoTable.OAADDS);
            //    AddPrintVariable("CustomerZip", OrderAddressInfoTable.OAADDZ);
            //    AddPrintVariable("CustomerCountryCode", OrderAddressInfoTable.OACTR);
            //}
            //else 
            if (AddressByDateTable.RetrieveAddressInfo(shipToAddressNumber))
            {
                AddPrintVariable("CustomerPlantName", AddressByDateTable["ABALPH"]);
                AddPrintVariable("CustomerPlantMailingName", AddressByDateTable["WWMLNM"]);
                AddPrintVariable("CustomerAddress1", AddressByDateTable.ALADD1);
                AddPrintVariable("CustomerAddress2", AddressByDateTable.ALADD2);
                AddPrintVariable("CustomerAddress3", AddressByDateTable.ALADD3);
                AddPrintVariable("CustomerAddress4", String.Format("{0}, {1} {2}", AddressByDateTable.ALCTY1, AddressByDateTable.ALADDS, AddressByDateTable.ALADDZ));
                AddPrintVariable("CustomerCity", AddressByDateTable.ALCTY1);
                AddPrintVariable("CustomerState", AddressByDateTable.ALADDS);
                AddPrintVariable("CustomerZip", AddressByDateTable.ALADDZ);
                AddPrintVariable("CustomerCountryCode", AddressByDateTable.ALCTR);
            }

            //If a valid Address Number was provided, the attempt to fetch
            //if ((addressNumber > -1) && (SupplementalDataTable.RetrieveAddressCode("EDI", addressNumber, shipToAddressNumber)))
            if ((addressNumber > -1) && (CustomerPlantTable.RetrieveAddressCode(branchPlant, addressNumber, shipToAddressNumber)))
            {
                //AddPrintVariable("CustomerPlantCode", SupplementalDataTable.T3RMK2);
                AddPrintVariable("CustomerPlantCode", CustomerPlantTable.ETDFBDESC);
            }

            AddBlankVariables("CustomerPlantName", "CustomerPlantMailingName", "CustomerAddress1", "CustomerAddress2", "CustomerAddress3", "CustomerAddress4", "CustomerCity", "CustomerState",
                "CustomerZip", "CustomerCountryCode", "CustomerPlantCode");
        }

        #endregion

        #region GenerateItemData

        /// <summary>
        /// Generates Item data
        /// </summary>
        /// <param name="branchPlant">Branch Plant (MCU)</param>
        /// <param name="shortItemNumber">Short Item Number (ITM)</param>
        public virtual void GenerateItemData(string branchPlant, double shortItemNumber)
        {
            WarehouseConstantsTable.DeterminePrimaryItem(branchPlant);

            if (ItemMasterTable.FetchItem(shortItemNumber))
            {
                switch (WarehouseConstantsTable._DesignatedPrimaryItemNumber)
                {
                    case PrimaryItemNumberDesignations.ShortNumber:
                        AddPrintVariable("PrimaryItemNumber", ItemMasterTable.IMITM);
                        break;
                    case PrimaryItemNumberDesignations.LongNumber:
                        AddPrintVariable("PrimaryItemNumber", ItemMasterTable.IMLITM);
                        break;
                    case PrimaryItemNumberDesignations.ThirdNumber:
                        AddPrintVariable("PrimaryItemNumber", ItemMasterTable.IMAITM);
                        break;
                }

                AddPrintVariable("ShortItemNumber", ItemMasterTable.IMITM);
                AddPrintVariable("LongItemNumber", ItemMasterTable.IMLITM);
                AddPrintVariable("ThirdItemNumber", ItemMasterTable.IMAITM);
                AddPrintVariable("ItemDescription1", ItemMasterTable.IMDSC1);
                AddPrintVariable("ItemDescription2", ItemMasterTable.IMDSC2);

                if (ItemBranchTable.ValidateItemBranch(branchPlant, shortItemNumber))
                {
                    AddPrintVariable("CountryOfOrigin", ItemBranchTable.IBORIG);
                }
            }

            AddBlankVariables("PrimaryItemNumber", "ShortItemNumber", "LongItemNumber", "ThirdItemNumber", "ItemDescription1", "ItemDescription2", "CountryOfOrigin");
        }

        #endregion

        #region GenerateLotData

        /// <summary>
        /// Generates Lot data
        /// </summary>
        /// <param name="branchPlant">Branch Plant (MCU)</param>
        /// <param name="shortItemNumber">Short Item Number (ITM)</param>
        /// <param name="lotNumber">Lot Number (LOTN)</param>
        public virtual void GenerateLotData(string branchPlant, double shortItemNumber, string lotNumber)
        {
            if (LotMasterTable.RetrieveLotMaster(branchPlant, shortItemNumber, lotNumber))
            {
                AddPrintVariable("LotNumber", LotMasterTable.IOLOTN);
                AddPrintVariable("LotExpirationDate", DateTimeUtils.JDEJulian(LotMasterTable.IOMMEJ).ToShortDateString());
            }

            AddBlankVariables("LotNumber", "LotExpirationDate");
        }

        #endregion

        #endregion

        #region ConvertQuantity

        /// <summary>
        /// Attempts to convert the quantity from the "From" Unit of Measure to the "To" Unit of Measure. Should
        /// no conversion be found, no conversion factor is applied to the quantity
        /// </summary>
        /// <param name="branch">Branch Plant</param>
        /// <param name="shortItemNumber">Short Item Number</param>
        /// <param name="fromUOM">From Unit of Measure</param>
        /// <param name="toUOM">To Unit of Measure</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>The Quantity, converted, if a conversion factor was found</returns>
        protected double ConvertQuantity(string branch, double shortItemNumber, string fromUOM, string toUOM, double quantity)
        {
            //The following is used to allow us to cache conversion factors. This is useful in case we are doing many calls to this code, it can
            //reduce the number of queries (the dictionary is also reset in ResetAll, which helps ensure the caching isn't too extreme)
            string currentKey = String.Format("{0}~{1}~{2}~{3}", branch, shortItemNumber, fromUOM, toUOM);

            if (!_ConversionFactors.ContainsKey(currentKey))
            {
                if (!fromUOM.Trim().Equals(toUOM.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    double conversionFactor = UnitOfMeasureConversionTable1.ConvertToPrimaryUOM(branch, shortItemNumber, fromUOM, toUOM);
                    if (conversionFactor == 0)
                    {
                        conversionFactor = UnitOfMeasureConversionTable2.ConvertToPrimaryUOM(fromUOM, toUOM);
                    }

                    //If a valid conversion factor was found, then store that value, but if not, then we store 1, as we have nothing to convert
                    //the value with, so we'll simply set a 1, to ensure the quantity is returned as is (and not 0 for instance)
                    _ConversionFactors.Add(currentKey, ((conversionFactor != 0) ? conversionFactor : 1));
                }
            }

            return (quantity * _ConversionFactors[currentKey]);
        }

        #endregion

        #region AddPrintVariable

        /// <summary>
        /// Adds a new print variable to the last row in the PrintData DataTable, should it not already exist. Else if
        /// it does, updates the value. This will also take into account if EnableMultiPartLabels is set to True, which
        /// if so, will append the current index onto the end of the Name
        /// </summary>
        /// <param name="name">Name to assign the value to</param>
        /// <param name="value">Value to print</param>
        public void AddPrintVariable(string name, object value)
        {
            SetPrintVariable(name, value, OverwriteMode.Current);
        }

        #endregion

        #region SetPrintVariable

        /// <summary>
        /// Adds a new print variable to the PrintData DataTable, setting the default value for each row currently in 
        /// the table to the blank, should it not alread exist. After which, the value a value may be assigned based
        /// on the set OverwriteMode
        /// </summary>
        /// <param name="name">Name to assign the value to</param>
        /// <param name="value">Value to print</param>
        /// <param name="overwriteMode">Overwrite Mode to control how (if at all), the value will be assigned</param>
        protected void SetPrintVariable(string name, object value, OverwriteMode overwriteMode)
        {
            name = (VariableNamePrefix + name); //add on the variable prefix

            if (EnableMultiPartLabels) //if enabled, append the part's index to the end of the variable name
            {
                name = String.Format("{0}_{1}", name, _CurrentPartsIndex);
            }

            if (!PrintDataTable.Columns.Contains(name))
            {
                //By creating this way, the default value applies to all rows now and any that are added later on
                using (DataColumn column = new DataColumn(name))
                {
                    column.DefaultValue = "";
                    PrintDataTable.Columns.Add(column);
                }
            }

            //Depending on the mode, we have the ability to specify if the current value is set to only the CurrentRow
            //or all the rows currently present. This is done because be default, each variable will be set to a blank
            switch (overwriteMode)
            {
                case OverwriteMode.Current:
                    CurrentDataRow[name] = value;
                    break;
                case OverwriteMode.All:
                    foreach (DataRow row in PrintDataTable.Rows)
                    {
                        row[name] = value;
                    }

                    break;
            }
        }

        #endregion

        #region AddBlankVariables

        /// <summary>
        /// Adds the collection of print variables as blanks, should they not already exist. This will also take into account if 
        /// EnableMultiPartLabels is set to True, which if so, will append the current index onto the end of the Name
        /// </summary>
        /// <param name="names">Collection of Names</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the names provided is of a null like value</exception>
        public void AddBlankVariables(params string[] names)
        {
            AddBlankVariables(false, names);
        }

        /// <summary>
        /// Adds the collection of print variables as blanks, should they not already exist. This will also take into account if 
        /// EnableMultiPartLabels is set to True, which if so, will append the current index onto the end of the Name
        /// </summary>
        /// <param name="overwrite">If True, all variables in the CurrentRow will be set to blank, even if another value already exists</param>
        /// <param name="names">Collection of Names</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the names provided is of a null like value</exception>
        public void AddBlankVariables(bool overwrite, params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }

            foreach (string name in names)
            {
                //Sets the variable to blank in the CurrentRow, overwriting the current value, depending on the bool flag (or just adding if it
                //doesn't already exist)
                SetPrintVariable(name, "", (overwrite ? OverwriteMode.Current : OverwriteMode.None));
            }
        }

        #endregion

        #region TransferPrintData

        /// <summary>
        /// Copies the contents of the PrintData DataTable over to the RFS_AdditionalPrintData DataTable in
        /// the provided DAL, clearing out any content currently present in there first
        /// </summary>
        /// <param name="dataAccess">Data Access Layer</param>
        public void TransferPrintData(DAL dataAccess)
        {
            if (!dataAccess.UserDS.Tables.Contains("RFS_AdditionalPrintData"))
            {
                using (DataTable table = new DataTable("RFS_AdditionalPrintData"))
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    dataAccess.UserDS.Tables.Add(table);
                }
            }

            dataAccess.UserDS.Tables["RFS_AdditionalPrintData"].Clear();
            dataAccess.UserDS.Tables["RFS_AdditionalPrintData"].Columns.Clear(); //clears out any existing data

            //We cannot use DataTable.Copy, because the base code will not allow us to "set" a DataTable within the UserDS. As
            //such we have to use this process below to copy over columns and then rows

            foreach (DataColumn column in PrintDataTable.Columns) //first copies over the columns
            {
                dataAccess.UserDS.Tables["RFS_AdditionalPrintData"].Columns.Add(column.ColumnName, column.DataType);
            }

            foreach (DataRow row in PrintDataTable.Rows) //now copy over all the rows
            {
                dataAccess.UserDS.Tables["RFS_AdditionalPrintData"].Rows.Add(dataAccess.UserDS.Tables["RFS_AdditionalPrintData"].NewRow());

                foreach (DataColumn column in PrintDataTable.Columns)
                {
                    dataAccess.UserDS.Tables["RFS_AdditionalPrintData"].Rows[dataAccess.UserDS.Tables["RFS_AdditionalPrintData"].Rows.Count - 1][column.ColumnName] = row[column.ColumnName];
                }
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of the Managed Resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Cleans up all the resources
        /// </summary>
        /// <param name="disposing">When True, we clean up Managed Resources as well as Unmanaged</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (_SupplementalDataTable != null)
                {
                    _SupplementalDataTable.Dispose();
                }

                if (_AddressBookMasterTable != null)
                {
                    _AddressBookMasterTable.Dispose();
                }

                if (_AddressByDateTable != null)
                {
                    _AddressByDateTable.Dispose();
                }

                if (_OrderAddressInfoTable != null)
                {
                    _OrderAddressInfoTable.Dispose();
                }

                if (_ItemMasterTable != null)
                {
                    _ItemMasterTable.Dispose();
                }

                if (_ItemBranchTable != null)
                {
                    _ItemBranchTable.Dispose();
                }

                if (_LotMasterTable != null)
                {
                    _LotMasterTable.Dispose();
                }

                if (_WarehouseConstantsTable != null)
                {
                    _WarehouseConstantsTable.Dispose();
                }

                if (_UnitOfMeasureConversionTable1 != null)
                {
                    _UnitOfMeasureConversionTable1.Dispose();
                }

                if (_UnitOfMeasureConversionTable2 != null)
                {
                    _UnitOfMeasureConversionTable2.Dispose();
                }

                if (_PrintDataTable != null)
                {
                    _PrintDataTable.Dispose();
                }
            }

            IsDisposed = true;
        }

        #endregion
    }
}