using System;
using System.Data;
using System.Linq;
using ICS.Database;
using ICS.Database.TableObjects.JDE;
using ICS.Utilities;
using ICS.Stoneridge.Wireless.JDE.Utilities.Extensions;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Utilities.Printing
{
    /// <summary>
    /// Dervied from the GenPrintDataBase, generates additional print variables focused around
    /// Shipments and Cartonization
    /// </summary>
    public class GenCartonPrintData : GenPrintDataBase
    {
        #region Class Level Variables

        #region Tables

        protected F0005 _UserDefinedCodesTable;

        protected F0006 _BranchPlantTable;

        protected CustomJDETables.F4104 _CustomerXrefTable;

        protected CustomJDETables.F4211 _SalesOrderDetailTable;

        protected CustomJDETables.F4620 _JDECartonTable;

        protected CustomJDETables.F4941 _ShipmentRoutingStepsTable;

        protected CustomJDETables.F40R11 _DemandDetailTable;

        protected CustomJDETables.F460131 _StdPackRecommendationRulesTable;

        protected CustomJDETables.F49211 _SaleOrderDetailTagTable;

        protected CustomJDETables.F5846001 _JDESerialNumberDetailTable;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new GenCartonPrintData
        /// </summary>
        /// <param name="dataAccess">Data Access Layer</param>
        public GenCartonPrintData(DAL dataAccess) : base(dataAccess)
        {
        }

        #endregion

        #region Properties

        #region Tables

        #region UserDefinedCodesTable (F0005)

        /// <summary>
        /// Get the User Defined Codes Table (F0005)
        /// </summary>
        protected virtual F0005 UserDefinedCodesTable
        {
            get { return (_UserDefinedCodesTable ?? (_UserDefinedCodesTable = new F0005(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region BranchPlantTable (F0006)

        /// <summary>
        /// Get the Branch Plant Table (F0006)
        /// </summary>
        protected virtual F0006 BranchPlantTable
        {
            get { return (_BranchPlantTable ?? (_BranchPlantTable = new F0006(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region CustomerXrefTable (F4104)

        /// <summary>
        /// Get the Customer Cross Reference Table (F4104)
        /// </summary>
        protected virtual CustomJDETables.F4104 CustomerXrefTable
        {
            get { return (_CustomerXrefTable ?? (_CustomerXrefTable = new CustomJDETables.F4104(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region SalesOrderDetailTable (F4211)

        /// <summary>
        /// Get the Sales Order Detail Table (F4211)
        /// </summary>
        protected virtual CustomJDETables.F4211 SalesOrderDetailTable
        {
            get { return (_SalesOrderDetailTable ?? (_SalesOrderDetailTable = new CustomJDETables.F4211(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region JDECartonTable (F4620)

        /// <summary>
        /// Get the JDE Carton Table (F4620)
        /// </summary>
        protected virtual CustomJDETables.F4620 JDECartonTable
        {
            get { return (_JDECartonTable ?? (_JDECartonTable = new CustomJDETables.F4620(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region ShipmentRoutingStepsTable (F4941)

        /// <summary>
        /// Get the Shipment Routing Steps Table (F4941)
        /// </summary>
        protected virtual CustomJDETables.F4941 ShipmentRoutingStepsTable
        {
            get { return (_ShipmentRoutingStepsTable ?? (_ShipmentRoutingStepsTable = new CustomJDETables.F4941(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region DemandDetailTable (F40R11)

        /// <summary>
        /// Get the Demand Detail Table (F40R11)
        /// </summary>
        protected virtual CustomJDETables.F40R11 DemandDetailTable
        {
            get { return (_DemandDetailTable ?? (_DemandDetailTable = new CustomJDETables.F40R11(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region StdPackRecommendationRulesTable (F460131)

        /// <summary>
        /// Get the Standard Pack Recommendation Rules Table (F460131)
        /// </summary>
        protected virtual CustomJDETables.F460131 StdPackRecommendationRulesTable
        {
            get { return (_StdPackRecommendationRulesTable ?? (_StdPackRecommendationRulesTable = new CustomJDETables.F460131(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region SalesOrderDetailTagTable (F49211)

        protected virtual CustomJDETables.F49211 SalesOrderDetailTagTable
        {
            get { return (_SaleOrderDetailTagTable ?? (_SaleOrderDetailTagTable = new CustomJDETables.F49211(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #region JDESerialNumberDetailTable (F5846001)

        protected virtual CustomJDETables.F5846001 JDESerialNumberDetailTable
        {
            get { return (_JDESerialNumberDetailTable ?? (_JDESerialNumberDetailTable = new CustomJDETables.F5846001(_DataAccess, "GenCartonPrintData_"))); }
        }

        #endregion

        #endregion

        #region RoutingValue

        /// <summary>
        /// Get or Set the "Routing" value. Defaulted to "eLSD"
        /// </summary>
        public string RoutingValue { get; set; }

        #endregion

        #endregion

        #region ResetAll

        /// <summary>
        /// Resets the PrintData DataTable back to a default state, with no data set yet, as well as all other
        /// dynamically adjustable variables
        /// </summary>
        public override void ResetAll()
        {
            RoutingValue = "eLSD";
            base.ResetAll();
        }

        #endregion

        #region Generate Data

        #region GenerateData

        /// <summary>
        /// Generates the Print Data for the Shipment
        /// </summary>
        /// <param name="shipmentNumber">Shipment Number (SHPN)</param>
        public void GenerateData(double shipmentNumber)
        {
            GenerateData(shipmentNumber, -1);
        }

        /// <summary>
        /// Generates the Print Data for the Shipment and Carton/Pallet provided 
        /// </summary>
        /// <param name="shipmentNumber">Shipment Number (SHPN)</param>
        /// <param name="cartonUKID">Carton/Pallet Unique ID (CDUKID)</param>
        public virtual void GenerateData(double shipmentNumber, double cartonUKID)
        {
            if (SalesOrderDetailTable.RetrieveShipmentLines(shipmentNumber))
            {
                AddPrintVariable("Routing", RoutingValue);

                //We must use the MCAN8 for most of the From Address information, except the Supplier Code, that must use the SDAN8 instead
                BranchPlantTable.FetchBranchPlant(SalesOrderDetailTable.SDMCU);
                BranchPlantConstantsTable.ResetAll();
                BranchPlantConstantsTable.SQL = $"SELECT CIAN8 FROM {BranchPlantConstantsTable.GetFullTableName()} ";
                BranchPlantConstantsTable.CIMCU = SalesOrderDetailTable.SDMCU;
                BranchPlantConstantsTable.Fetch();
                //GenerateShipFromAddress(BranchPlantTable.MCAN8, SalesOrderDetailTable.SDAN8, SalesOrderDetailTable.SDSHAN);
                GenerateShipFromAddress(BranchPlantConstantsTable.CIAN8, SalesOrderDetailTable.SDAN8, SalesOrderDetailTable.SDSHAN, SalesOrderDetailTable.SDMCU);

                GenerateShipToAddress(SalesOrderDetailTable.SDDOCO, SalesOrderDetailTable.SDDCTO, SalesOrderDetailTable.SDKCOO, SalesOrderDetailTable.SDSHAN, SalesOrderDetailTable.SDAN8, SalesOrderDetailTable.SDMCU);

                if (SalesOrderDetailTable.SDADDJ > 0) //if true, the current line has been ship confirmed
                {
                    SetJulianDateValues("Ship", SalesOrderDetailTable.SDADDJ);
                }
                else
                {
                    //Should the fetch fail, then we pass in -1, which sets the values to blank
                    SetJulianDateValues("Ship", ((ShipmentRoutingStepsTable.GetRouteForShipmentNumber(SalesOrderDetailTable.SDSHPN)) ? ShipmentRoutingStepsTable.RSPPDJ : -1));
                }

                AddPrintVariable("PackingList", SalesOrderDetailTable.SDSHPN);
                AddPrintVariable("BillOfLading", SalesOrderDetailTable.SDSHPN);
                AddPrintVariable("ShipmentID", SalesOrderDetailTable.SDSHPN);
                
                //_Log.Debug($"<***> SDDOC = {SalesOrderDetailTable.SDDOC}");
                AddPrintVariable("InvoiceNumber", SalesOrderDetailTable.SDDOC);

                if (cartonUKID > -1) //if a carton is provided, generate additional data around it
                {
                    //Retrieve the carton record, making sure in the process it exists for the specified shipment
                    if (JDECartonTable.RetrieveCartonUKID(cartonUKID, shipmentNumber))
                    {
                        _Log.Debug($"<***> JDECartonTable.CDUKID = {JDECartonTable.CDUKID}");
                        //Use the Item number to determine if a pallet or carton. This is done because if a Carton is on its own (aka
                        //not on a Pallet), then its CROS = 0. The Item number should be a safe tell though
                        if (JDECartonTable.CDITM == 0)
                        {
                            GeneratePalletData(JDECartonTable.CDUKID);
                        }
                        else
                        {
                            GenerateCartonData(JDECartonTable.CDUKID);
                        }
                    }
                    else
                    {
                        if (JDECartonTable.RetrieveCartonUKID(cartonUKID))
                        {
                            throw new RFS_Exception("ShipmentMismatch");
                        }

                        throw new RFS_Exception("CARTON");
                    }
                }
            }
            else
            {
                throw new RFS_Exception("SHIPMENT");
            }
        }

        public virtual void GenerateData(double shipmentNumber, double cartonUKID, double shortItemNumber)
        {
            if (SalesOrderDetailTable.RetrieveShipmentLines(shipmentNumber))
            {
                AddPrintVariable("Routing", RoutingValue);

                //We must use the MCAN8 for most of the From Address information, except the Supplier Code, that must use the SDAN8 instead
                BranchPlantTable.FetchBranchPlant(SalesOrderDetailTable.SDMCU);
                BranchPlantConstantsTable.ResetAll();
                BranchPlantConstantsTable.SQL = $"SELECT CIAN8 FROM {BranchPlantConstantsTable.GetFullTableName()} ";
                BranchPlantConstantsTable.CIMCU = SalesOrderDetailTable.SDMCU;
                BranchPlantConstantsTable.Fetch();
                //GenerateShipFromAddress(BranchPlantTable.MCAN8, SalesOrderDetailTable.SDAN8, SalesOrderDetailTable.SDSHAN);
                GenerateShipFromAddress(BranchPlantConstantsTable.CIAN8, SalesOrderDetailTable.SDAN8, SalesOrderDetailTable.SDSHAN, SalesOrderDetailTable.SDMCU);

                GenerateShipToAddress(SalesOrderDetailTable.SDDOCO, SalesOrderDetailTable.SDDCTO, SalesOrderDetailTable.SDKCOO, SalesOrderDetailTable.SDSHAN, SalesOrderDetailTable.SDAN8, SalesOrderDetailTable.SDMCU);

                if (SalesOrderDetailTable.SDADDJ > 0) //if true, the current line has been ship confirmed
                {
                    SetJulianDateValues("Ship", SalesOrderDetailTable.SDADDJ);
                }
                else
                {
                    //Should the fetch fail, then we pass in -1, which sets the values to blank
                    SetJulianDateValues("Ship", ((ShipmentRoutingStepsTable.GetRouteForShipmentNumber(SalesOrderDetailTable.SDSHPN)) ? ShipmentRoutingStepsTable.RSPPDJ : -1));
                }

                AddPrintVariable("PackingList", SalesOrderDetailTable.SDSHPN);
                AddPrintVariable("BillOfLading", SalesOrderDetailTable.SDSHPN);
                AddPrintVariable("ShipmentID", SalesOrderDetailTable.SDSHPN);
                //_Log.Debug($"<***> SDDOC = {SalesOrderDetailTable.SDDOC}");
                AddPrintVariable("InvoiceNumber", SalesOrderDetailTable.SDDOC);

                if (cartonUKID > -1) //if a carton is provided, generate additional data around it
                {
                    //Retrieve the carton record, making sure in the process it exists for the specified shipment
                    if (JDECartonTable.RetrieveCartonUKID(cartonUKID, shipmentNumber))
                    {
                        _Log.Debug($"<***> JDECartonTable.CDUKID = {JDECartonTable.CDUKID}");
                        //Use the Item number to determine if a pallet or carton. This is done because if a Carton is on its own (aka
                        //not on a Pallet), then its CROS = 0. The Item number should be a safe tell though
                        if (JDECartonTable.CDITM == 0)
                        {
                            GeneratePalletData(JDECartonTable.CDUKID, shortItemNumber);
                        }
                        else
                        {
                            GenerateCartonData(JDECartonTable.CDUKID);
                        }
                    }
                    else
                    {
                        if (JDECartonTable.RetrieveCartonUKID(cartonUKID))
                        {
                            throw new RFS_Exception("ShipmentMismatch");
                        }

                        throw new RFS_Exception("CARTON");
                    }
                }
            }
            else
            {
                throw new RFS_Exception("SHIPMENT");
            }
        }

        #endregion

        #region GeneratePalletData

        /// <summary>
        /// Generates the data for a Pallet Label (or Master Label)
        /// </summary>
        /// <param name="palletUKID">Pallet Unique ID</param>
        public virtual void GeneratePalletData(double palletUKID)
        {
            //We use a temp table to hold onto the pallet information that is fetched, while grabbing carton data
            using (CustomJDETables.F4620 palletTable = new CustomJDETables.F4620(_DataAccess, "GenCartonPrintData_Pallet_"))
            {
                if (!palletTable.RetrieveCartonUKID(palletUKID))
                {
                    throw new RFS_Exception("PALLET");
                }

                JDECartonTable.FetchDistinctPalletItems(palletUKID);

                if (JDECartonTable.Rows.Count > 1) //if more then 1, it's a mixed pallet, so enable the incrementing code (no limit on these however)
                {
                    EnableMultiPartLabels = true;
                }

                //Now increment through all the items. We use the enumerable so if we re-quiry the F4620 below, we don't
                //risk losing this collection. Note this is done because if a Mixed we need the extra data for a Mixed Master, while
                //if a Carton, some of this info may still be useful
                foreach (double id in JDECartonTable.ThisDataTable.ColumnToEnumerable<double>("CDUKID").ToArray())
                {
                    GenerateCartonData(id);
                    IncrementPartsIndex(true);
                }

                EnableMultiPartLabels = false; //must make sure to set back before setting anymore (in case Mixed, it allows these to exist along with the Cartons)

                //Any variables below that share the same names as those in the carton data will be overwritten (if applicable, aka not a mixed)

                AddPrintVariable("SerialNumber", palletTable.CDPLT);
                AddPrintVariable("BoxQuantity", palletTable.GetPalletBoxQuantity(palletUKID, true));


                double pounds = 0;
                double summedKilo = 0;
                double summedPounds = 0;

                //Fetches all the picked Cartons that are on the pallet, and indexes through them all, summing quantities. The reason we do it this
                //way instead of summing in SQL is because if it's a mixed, then there is the possibility UOM conversions may vary between items
                palletTable.FetchPalletsCartons(palletUKID, true);
                foreach (CustomJDETables.F4620 carton in palletTable)
                {
                    //Like below, there probably won't be a conversion from the CDUOM (most likely "EA") to "KG". To get around this, we'll instead go from
                    //"LB" to "KG", which should have a conversion factor
                    pounds = ConvertQuantity(carton.CDMCU, carton.CDITM, carton.CDUOM, "KG", carton.CDGRWT);
                    summedPounds += pounds;
                    summedKilo += ConvertQuantity(carton.CDMCU, carton.CDITM, "LB", "KG", pounds);
                }

                AddPrintVariable("BoxWeightPounds", summedPounds);
                AddPrintVariable("BoxWeightKilo", summedKilo);
            }
        }

        public virtual void GeneratePalletData(double palletUKID, double shortItemNumber)
        {
            //We use a temp table to hold onto the pallet information that is fetched, while grabbing carton data
            using (CustomJDETables.F4620 palletTable = new CustomJDETables.F4620(_DataAccess, "GenCartonPrintData_Pallet_"))
            {
                if (!palletTable.RetrieveCartonUKID(palletUKID))
                {
                    throw new RFS_Exception("PALLET");
                }

                JDECartonTable.FetchDistinctPalletItems(palletUKID, shortItemNumber);

                if (JDECartonTable.Rows.Count > 1) //if more then 1, it's a mixed pallet, so enable the incrementing code (no limit on these however)
                {
                    EnableMultiPartLabels = true;
                }

                //Now increment through all the items. We use the enumerable so if we re-quiry the F4620 below, we don't
                //risk losing this collection. Note this is done because if a Mixed we need the extra data for a Mixed Master, while
                //if a Carton, some of this info may still be useful
                foreach (double id in JDECartonTable.ThisDataTable.ColumnToEnumerable<double>("CDUKID").ToArray())
                {
                    GenerateCartonData(id);
                    IncrementPartsIndex(true);
                }

                EnableMultiPartLabels = false; //must make sure to set back before setting anymore (in case Mixed, it allows these to exist along with the Cartons)

                //Any variables below that share the same names as those in the carton data will be overwritten (if applicable, aka not a mixed)

                AddPrintVariable("SerialNumber", palletTable.CDPLT);
                AddPrintVariable("BoxQuantity", palletTable.GetPalletBoxQuantity(palletUKID, true, shortItemNumber));

                double pounds = 0;
                double summedKilo = 0;
                double summedPounds = 0;

                //Fetches all the picked Cartons that are on the pallet, and indexes through them all, summing quantities. The reason we do it this
                //way instead of summing in SQL is because if it's a mixed, then there is the possibility UOM conversions may vary between items
                palletTable.FetchPalletsCartons(palletUKID, true);
                foreach (CustomJDETables.F4620 carton in palletTable)
                {
                    //Like below, there probably won't be a conversion from the CDUOM (most likely "EA") to "KG". To get around this, we'll instead go from
                    //"LB" to "KG", which should have a conversion factor
                    pounds = ConvertQuantity(carton.CDMCU, carton.CDITM, carton.CDUOM, "KG", carton.CDGRWT);
                    summedPounds += pounds;
                    summedKilo += ConvertQuantity(carton.CDMCU, carton.CDITM, "LB", "KG", pounds);
                }

                AddPrintVariable("BoxWeightPounds", summedPounds);
                AddPrintVariable("BoxWeightKilo", summedKilo);
            }
        }

        #endregion

        #region GenerateCartonData

        /// <summary>
        /// Generates the data for a Carton Label
        /// </summary>
        /// <param name="cartonUKID">Carton Unique ID (CDUKID)</param>
        public virtual void GenerateCartonData(double cartonUKID)
        {
            if (!JDECartonTable.RetrieveCartonUKID(cartonUKID)) //retrieve the F4620 data for the carton
            {
                throw new RFS_Exception("CARTON");
            }

            _Log.Debug($"<***> CDPAK = {JDECartonTable.CDPAK}");
            AddPrintVariable("SerialNumber", JDECartonTable.CDPAK);
            AddPrintVariable("CustomerReturnNumber", JDECartonTable.CDCEQT);

            //Now pull back the Sales Order Line for the Carton record (assume it will always exist)
            SalesOrderDetailTable.FetchSalesOrderLine(JDECartonTable.CDDOCO, JDECartonTable.CDDCTO, JDECartonTable.CDKCOO, JDECartonTable.CDLNID);

            AddPrintVariable("RAN", SalesOrderDetailTable.SDPSIG);
            AddPrintVariable("SRIPartNumber", SalesOrderDetailTable.SDLITM);
            AddPrintVariable("UPC", ItemMasterTable.IMUPCN);
            AddPrintVariable("PurchaseOrder", SalesOrderDetailTable.SDVR01);

            if (DemandDetailTable.RetrieveDemand(SalesOrderDetailTable.SDDEID))
            {
                AddPrintVariable("DockCode", DemandDetailTable.DDDKID);
                AddPrintVariable("LineFeed", DemandDetailTable.DDASBLNFD);
                AddPrintVariable("ReserveLineFeed", DemandDetailTable.DDRSVLNFD);
                AddPrintVariable("PurchaseOrderLine", DemandDetailTable.DDCSTPOLIN);
            }

            GenerateItemData(SalesOrderDetailTable.SDMCU, SalesOrderDetailTable.SDITM);
            GenerateCustomerItemData(SalesOrderDetailTable.SDAN8, SalesOrderDetailTable.SDITM, SalesOrderDetailTable.SDTRDJ);
            GenerateLotData(JDECartonTable.CDMCU, JDECartonTable.CDITM, JDECartonTable.CDPAK);
            GenerateGeneralData(SalesOrderDetailTable.SDDEID);

            //There probably won't be a conversion from the CDUOM (like "EA") to "KG". To get around this, we'll instead go from "LB" to "KG", which should have a conversion factor
            double pounds = ConvertQuantity(JDECartonTable.CDMCU, JDECartonTable.CDITM, JDECartonTable.CDUOM, "LB", JDECartonTable.CDGRWT);
            AddPrintVariable("BoxWeightPounds", pounds);
            AddPrintVariable("BoxWeightKilo", ConvertQuantity(JDECartonTable.CDMCU, JDECartonTable.CDITM, "LB", "KG", pounds));

            double stdPack = StdPackRecommendationRulesTable.RetrieveStandardPack(JDECartonTable.CDITM, SalesOrderDetailTable.SDAN8, SalesOrderDetailTable.SDSHAN);
            if (stdPack == 0)
            {
                stdPack = StdPackRecommendationRulesTable.RetrieveStandardPack(JDECartonTable.CDITM, SalesOrderDetailTable.SDAN8);
                if (stdPack == 0)
                {
                    stdPack = StdPackRecommendationRulesTable.RetrieveStandardPack(JDECartonTable.CDITM);
                }
            }

            AddPrintVariable("StandardPack", stdPack);

            if (JDECartonTable.CDCROS > 0) //if assigned to a pallet, find all the occurances of the item on the pallet
            {
                using (CustomJDETables.F4620 temp = new CustomJDETables.F4620(_DataAccess, "Temp_"))
                {
                    if (temp.RetrieveCartonItemQty(JDECartonTable.CDMCU, JDECartonTable.CDITM, JDECartonTable.CDCROS))
                    {
                        AddPrintVariable("CurrentBoxCount", temp["TotalCount"]);

                        if (stdPack > 0) //if a standard pack was pulled back successfully, then we divide (if not it would be 0, which u can't divide by)
                        {
                            AddPrintVariable("TotalBoxCount", temp["TotalCount"]);
                            //AddPrintVariable("TotalBoxCount", (Convert.ToDouble(temp ["TotalBoxQuantity"]) / stdPack));
                        }
                    }

                    AddPrintVariable("BoxQuantity", temp.GetBoxQuantity(JDECartonTable.CDSHPN, JDECartonTable.CDCTSN));
                    //revisar
                    _Log.Debug("#*$ GET Total Pallets :"+ JDECartonTable.CDMCU+"Ship:"+JDECartonTable.CDSHPN);

                    temp.GetNumberOfPallets(JDECartonTable.CDMCU, JDECartonTable.CDSHPN);
                    _Log.Debug("#*$ GET Total Pallets count :"+temp.Rows.Count.ToString());

                    AddPrintVariable("TotalCount",temp.Rows.Count.ToString() );


                }
            }

            if (SalesOrderDetailTagTable.FetchJobAndSeqNumber(SalesOrderDetailTable.SDDOCO, SalesOrderDetailTable.SDDCTO, SalesOrderDetailTable.SDKCOO, SalesOrderDetailTable.SDLNID))
            {
                AddPrintVariable("JobNumber", SalesOrderDetailTagTable.UDPSJOBNO);
                AddPrintVariable("JobSequence", SalesOrderDetailTagTable.UDJOBSQ);
            }
            else
            {
                AddPrintVariable("JobNumber", "");
                AddPrintVariable("JobSequence", "");
            }

            //For any of the fetches and such above, which if failed, wouldn't populate the varibles, we make sure they are still set
            AddBlankVariables("DockCode", "LineFeed", "ReserveLineFeed", "PurchaseOrderLine", "CurrentBoxCount", "TotalBoxCount", "BoxQuantity");
        }

        #endregion

        #region GenerateCustomerItemData

        /// <summary>
        /// Generates the Customer Item data
        /// </summary>
        /// <param name="addressNumber">Address Number (AN8)</param>
        /// <param name="shortItemNumber">Short Item Number (ITM)</param>
        /// <param name="julianOrderDate">Order Date (TRDJ) in Julian format</param>
        public virtual void GenerateCustomerItemData(double addressNumber, double shortItemNumber, decimal julianOrderDate)
        {
            if (CustomerXrefTable.RetrieveCustomerItem(addressNumber, shortItemNumber, "C", julianOrderDate))
            {
                AddPrintVariable("CustomerPartNumber", CustomerXrefTable.IVCITM);
                AddPrintVariable("PartDesc", CustomerXrefTable.IVDSC2);
            }

            if (CustomerXrefTable.RetrieveCustomerItem(addressNumber, shortItemNumber, "RL", julianOrderDate))
            {
                AddPrintVariable("CustomerRevision", CustomerXrefTable.IVCITM);
            }

            AddBlankVariables("CustomerPartNumber", "PartDesc", "CustomerRevision");
        }

        #endregion

        #region GenerateLotDate

        /// <summary>
        /// Generates Lot data
        /// </summary>
        /// <param name="branchPlant">Branch Plant (MCU)</param>
        /// <param name="shortItemNumber">Short Item Number (ITM)</param>
        /// <param name="lotNumber">Lot Number (LOTN)</param>
        public override void GenerateLotData(string branchPlant, double shortItemNumber, string serialNumber)
        {
            if (JDESerialNumberDetailTable.RetrieveMfgDate(branchPlant, shortItemNumber, serialNumber))
            {
                try
                {
                    SetJulianDateValues("Mfg", decimal.Parse(JDESerialNumberDetailTable.LPTCDE.ToString()));
                }
                catch
                {
                }
            }

            //if (LotMasterTable.RetrieveLotMaster(branchPlant, shortItemNumber, serialNumber))
            //{
            //   AddPrintVariable("LotNumber", LotMasterTable.IOLOTN);
            //    AddPrintVariable("LotExpirationDate", DateTimeUtils.JDEJulian(LotMasterTable.IOMMEJ).ToShortDateString());

            //    SetJulianDateValues("Mfg", LotMasterTable.IOBODJ);
            //}

            AddBlankVariables("LotNumber", "LotExpirationDate");
            SetJulianDateValues("Mfg", -1); //will set blank values if the fetch above fails 
        }

        #endregion

        #region GenerateGeneralData

        /// <summary>
        /// Gets the General data
        /// </summary>
        /// <param name="demandUKID">Demand Unique ID (DEID)</param>
        public virtual void GenerateGeneralData(double demandUKID)
        {
            if (SupplementalDataTable.RetrieveGeneralData(demandUKID))
            {
                foreach (DataRow row in SupplementalDataTable.ThisDataTable.Rows)
                {
                    SupplementalDataTable.CurrentRow = row;

                    //Because a series of unions were used we add this check in case the possibility one of the 
                    //union fetches is to fail (we make sure the values match up correctly)
                    switch (SupplementalDataTable.T3KY.Trim().ToUpperInvariant())
                    {
                        case "11Z":
                            AddPrintVariable("GeneralData1", SupplementalDataTable.T3RMK);
                            break;
                        case "12Z":
                            AddPrintVariable("GeneralData2", SupplementalDataTable.T3RMK);
                            break;
                        case "13Z":
                            AddPrintVariable("GeneralData3", SupplementalDataTable.T3RMK);
                            break;
                        case "14Z":
                            AddPrintVariable("GeneralData4", SupplementalDataTable.T3RMK);
                            break;
                        case "15Z":
                            AddPrintVariable("GeneralData5", SupplementalDataTable.T3RMK);
                            break;
                        case "16Z":
                            AddPrintVariable("GeneralData6", SupplementalDataTable.T3RMK);
                            break;
                        case "17Z":
                            AddPrintVariable("GeneralData7", SupplementalDataTable.T3RMK);
                            break;
                        case "18Z":
                            AddPrintVariable("GeneralData8", SupplementalDataTable.T3RMK);
                            break;
                        case "19Z":
                            AddPrintVariable("GeneralData9", SupplementalDataTable.T3RMK);
                            break;
                        case "20Z":
                            AddPrintVariable("GeneralData10", SupplementalDataTable.T3RMK);
                            break;
                    }
                }
            }

            //Adds any not currently present (this may include some where the fetch above may have a Union fail)
            AddBlankVariables("GeneralData1", "GeneralData2", "GeneralData3", "GeneralData4", "GeneralData5", "GeneralData6", "GeneralData7", "GeneralData8", "GeneralData9", "GeneralData10");
        }

        #endregion

        #endregion

        #region SetJulianDateValues

        /// <summary>
        /// Sets up a series of print variables based on the provided Julian Date
        /// </summary>
        /// <param name="prefix">Prefix to append to the beginning of the variable names</param>
        /// <param name="julianDate">Julian Date</param>
        protected void SetJulianDateValues(string prefix, decimal julianDate)
        {
            if (julianDate > -1) //used as a fail-safe, allowing the need to generate just the blank values if needed
            {
                DateTime date = DateTimeUtils.JDEJulian(julianDate);

                AddPrintVariable(String.Format("{0}DateJulian", prefix), julianDate);
                AddPrintVariable(String.Format("{0}DateMonth", prefix), date.ToString("MM"));
                AddPrintVariable(String.Format("{0}DateDay", prefix), date.ToString("dd"));
                AddPrintVariable(String.Format("{0}DateYear", prefix), date.ToString("yyyy"));

                if (UserDefinedCodesTable.ValidateUDC("55", "MN", date.ToString("MM")))
                {
                    AddPrintVariable(String.Format("{0}DateMonth3", prefix), UserDefinedCodesTable.DRDL01);
                }
            }

            AddBlankVariables(String.Format("{0}DateJulian", prefix), String.Format("{0}DateMonth", prefix), String.Format("{0}DateDay", prefix), String.Format("{0}DateYear", prefix),
                String.Format("{0}DateMonth3", prefix));
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Cleans up all the resources
        /// </summary>
        /// <param name="disposing">When True, we clean up Managed Resources as well as Unmanaged</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_UserDefinedCodesTable != null)
                {
                    _UserDefinedCodesTable.Dispose();
                }

                if (_BranchPlantTable != null)
                {
                    _BranchPlantTable.Dispose();
                }

                if (_BranchPlantTable != null)
                {
                    _BranchPlantTable.Dispose();
                }

                if (_CustomerXrefTable != null)
                {
                    _CustomerXrefTable.Dispose();
                }

                if (_SalesOrderDetailTable != null)
                {
                    _SalesOrderDetailTable.Dispose();
                }

                if (_JDECartonTable != null)
                {
                    _JDECartonTable.Dispose();
                }

                if (_ShipmentRoutingStepsTable != null)
                {
                    _ShipmentRoutingStepsTable.Dispose();
                }

                if (_DemandDetailTable != null)
                {
                    _DemandDetailTable.Dispose();
                }

                if (_StdPackRecommendationRulesTable != null)
                {
                    _StdPackRecommendationRulesTable.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}