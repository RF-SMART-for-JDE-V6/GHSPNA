using ICS.Database.TableObjects.JDE;
using ICS.Database.TableObjects.RFS;
using ICS.Display;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Inventory
{
    /// <summary>
    /// Screen Control Class For: StoneridgeInventoryInquiry Workflow
    /// </summary>
    public class StoneridgeInventoryInquiry : ICS.Workflow.Base.WorkflowDesign.Components.RFSWorkflowDesigner
    {
        #region Class Level defs

        #region Tables

        private F0006 _BranchPlantTable;

        private F4101 _ItemMasterTable;

        private F4100 _LocationMasterTable;

        private F4104 _ItemCrossReferenceTable;

        private F41001 _WarehouseConstants;

        private F41002 _UnitOfMeasureConversionTable1;

        private F41003 _UnitOfMeasureConversionTable2;

        private F41021 _ItemLocationTable;

        private RFS_DataManipulator _DataManipulatorTable;

        #endregion

        #region Screens

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectBranch;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectItemNumber;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLocation;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _DisplayItemInformation;

        // list/lookup screens
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _BranchPlantList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _ItemMasterList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _ItemLocationList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _LocationMasterList;

        #endregion

        #region Collected/Working Data

        private string _BranchPlant;

        private string _PrimaryItemNumber;

        private double _ShortItemNumber;

        private string _LotNumber;

        private string _Location;

        #region Workflow Options

        private string _InquiryType;

        private string _ShowZeroOnHand;

        #endregion

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Workflow : StoneridgeInventoryInquiry
        /// </summary>
        /// <param name="userSession">Contains UserSession info for controling screen display and DAL.</param>
        public StoneridgeInventoryInquiry(UserSession userSession)
            : base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeInventoryInquiry()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes Workflow and Sets screens and Table Objects.
        /// Declares Next with starting screen
        /// </summary>
        protected override void Initialize()
        {
            #region Table

            this._BranchPlantTable = new F0006(UserSession.DataAccess, "");
            this._LocationMasterTable = new F4100(UserSession.DataAccess, "");
            this._ItemMasterTable = new F4101(UserSession.DataAccess, "");
            this._ItemCrossReferenceTable = new F4104(UserSession.DataAccess, "");
            this._WarehouseConstants = new F41001(UserSession.DataAccess, "");
            this._UnitOfMeasureConversionTable1 = new F41002(UserSession.DataAccess, "");
            this._UnitOfMeasureConversionTable2 = new F41003(UserSession.DataAccess, "");
            this._ItemLocationTable = new F41021(UserSession.DataAccess, "");
            this._DataManipulatorTable = new RFS_DataManipulator(UserSession.DataAccess, "");

            #endregion

            #region Workflow Options

            this._InquiryType = WFOptions.RetrieveOptionValue("InquiryType").Trim().ToUpper();
            this._ShowZeroOnHand = WFOptions.RetrieveOptionValue("ShowZeroOnHand").Trim().ToUpper();

            #endregion

            _ItemLocationTable.OmitHeldLots = "Y";
        }

        #endregion

        #region InitializeComponent

        private void InitializeComponent()
        {
            this._LocationMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectBranch = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectItemNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ItemMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ItemLocationList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._DisplayItemInformation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // _LocationMasterList
            // 
            this._LocationMasterList.RfsWorkflow = this;
            this._LocationMasterList.NextScreen = this._ItemLocationList;
            this._LocationMasterList.ScreenName = "F4100_List";
            this._LocationMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LocationMasterList_LoadEvent);
            this._LocationMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LocationMasterList_EnterEvent);
            // 
            // _CollectBranch
            // 
            this._CollectBranch.NextScreen = this._CollectItemNumber;
            this._CollectBranch.RfsWorkflow = this;
            this._CollectBranch.ScreenName = "CollectBranch";
            this._CollectBranch.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranch_EnterEvent);
            this._CollectBranch.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranch_ListEvent);
            // 
            // _BranchPlantList
            // 
            this._BranchPlantList.RfsWorkflow = this;
            this._BranchPlantList.ScreenName = "F0006_List";
            this._BranchPlantList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BranchPlantList_LoadEvent);
            this._BranchPlantList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BranchPlantList_EnterEvent);
            // 
            // _CollectItemNumber
            // 
            this._CollectItemNumber.NextScreen = this._ItemLocationList;
            this._CollectItemNumber.RfsWorkflow = this;
            this._CollectItemNumber.ScreenName = "CollectItemNumber";
            this._CollectItemNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectItemNumber_EnterEvent);
            this._CollectItemNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectItemNumber_ListEvent);
            // 
            // _ItemMasterList
            // 
            this._ItemMasterList.RfsWorkflow = this;
            this._ItemMasterList.ScreenName = "F4101_List";
            this._ItemMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemMasterList_LoadEvent);
            this._ItemMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemMasterList_EnterEvent);
            // 
            // _ItemLocationList
            // 
            this._ItemLocationList.NextScreen = this._DisplayItemInformation;
            this._ItemLocationList.RfsWorkflow = this;
            this._ItemLocationList.ScreenName = "F41021_List";
            this._ItemLocationList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemLocationList_EnterEvent);
            this._ItemLocationList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemLocationList_LoadEvent);
            // 
            // _DisplayItemInformation
            // 
            this._DisplayItemInformation.RfsWorkflow = this;
            this._DisplayItemInformation.ScreenName = "DisplayItemInformation";
            this._DisplayItemInformation.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.DisplayItemInformation_EnterEvent);
            this._DisplayItemInformation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.DisplayItemInformation_EnterEvent);
            // 
            // _CollectLocation
            // 
            this._CollectLocation.NextScreen = this._ItemLocationList;
            this._CollectLocation.RfsWorkflow = this;
            this._CollectLocation.ScreenName = "CollectLocation";
            this._CollectLocation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_EnterEvent);
            this._CollectLocation.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_ListEvent);
            // 
            // StoneridgeInventoryInquiry
            // 
            this.StartScreen = this._CollectBranch;
            this.WorkflowID = "StoneridgeInventoryInquiry";
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            UserSession.ScreenHandler.ScreenMap.ClearAllWorkflows();
            UserSession.ScreenHandler.ScreenMap.Clear("Error");
            base.Dispose();
        }

        #endregion

        #region Methods

        #region CollectBranch

        /// <summary>
        /// Validates the user's input is a valid branch plant
        /// </summary>
        private void CollectBranch_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_BranchPlantTable.FetchBranchPlant(e.UserInput.ToString()))
            {
                CollectBranch_Enter();
            }
            else
            {
                throw new RFS_Exception("MCU");
            }
        }

        /// <summary>
        /// Updates the selected branch plant, then fetches the constants and determines type of inquiry
        /// </summary>
        private void CollectBranch_Enter()
        {
            _BranchPlant = _BranchPlantTable.MCMCU;
            _WarehouseConstants.DeterminePrimaryItem(_BranchPlant);

            //Determine if we are inquirying by Item or not
            Next((_InquiryType == "I") ? _CollectItemNumber : _CollectLocation);
            return;
        }

        /// <summary>
        /// Calles the F0006 List
        /// </summary>
        private void CollectBranch_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _BranchPlantList.Show();
        }

        #endregion

        #region BranchPlantList (F0006)

        /// <summary>
        /// Queries the F0006 for all available branch plants
        /// </summary>
        private void BranchPlantList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _BranchPlantTable.FetchBranchPlants();
        }

        /// <summary>
        /// Updates the user's selected row and moves on
        /// </summary>
        private void BranchPlantList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _BranchPlantTable.UpdateCurrentRow();
            CollectBranch_Enter();
        }

        #endregion

        #region CollectItemNumber

        /// <summary>
        /// Simply makes sure the item number isn't blank
        /// </summary>
        private void CollectItemNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToString().Trim().Length > 0)
            {
                CollectItemNumber_Enter(e.UserInput.ToString());
            }
            else
            {
                throw new RFS_Exception("BLANKSNOTALLOWED");
            }
        }

        /// <summary>
        /// Validates the item number, retrieving it's primary
        /// </summary>
        /// <param name="ItemNumber">Enter or Selected Item Number</param>
        private void CollectItemNumber_Enter(string ItemNumber)
        {
            _PrimaryItemNumber = _ItemMasterTable.RetrieveItem(_BranchPlant, ItemNumber, _WarehouseConstants._DesignatedPrimaryItemNumber.ToString());
            if (_PrimaryItemNumber != null)
            {
                _ShortItemNumber = _ItemMasterTable.IMITM;
                switch (_WarehouseConstants._DesignatedPrimaryItemNumber)
                {
                    case PrimaryItemNumberDesignations.ShortNumber:
                        UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMITM.ToString());
                        break;
                    case PrimaryItemNumberDesignations.LongNumber:
                        UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMLITM);
                        break;
                    case PrimaryItemNumberDesignations.ThirdNumber:
                        UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMAITM);
                        break;
                }

                Next(_ItemLocationList);
                return;
            }
            else
            {
                throw new RFS_Exception("ITEMNUMBER");
            }
        }

        /// <summary>
        /// Calls the F4101 List
        /// </summary>
        private void CollectItemNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemMasterList.Show();
        }

        #endregion

        #region ItemMasterList (F4101)

        /// <summary>
        /// Queries the F4101 for items in the specified branch
        /// </summary>
        private void ItemMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemMasterTable.ShowItemsInBranch(_BranchPlant, "");
        }

        /// <summary>
        /// Simply updates the row to the user's selected and passes the ItemNumber off
        /// for validation related items
        /// </summary>
        private void ItemMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemMasterTable.UpdateCurrentRow();
            CollectItemNumber_Enter(_ItemMasterTable.IMITM.ToString());
        }

        #endregion

        #region CollectLocation

        /// <summary>
        /// Validates the user's entered value is a valid location
        /// </summary>
        private void CollectLocation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_LocationMasterTable.ValidateLocation(_BranchPlant, e.UserInput.ToString()))
            {
                CollectLocation_Enter();
            }
            else
            {
                throw new RFS_Exception("LOCATION");
            }
        }

        /// <summary>
        /// Updates the values
        /// </summary>
        private void CollectLocation_Enter()
        {
            _Location = _LocationMasterTable.LMLOCN;
            UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _LocationMasterTable.sscnlocn);
        }

        /// <summary>
        /// Calls the F4100 List
        /// </summary>
        private void CollectLocation_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LocationMasterList.Show();
        }

        #endregion

        #region LocationMasterList (F4100)

        /// <summary>
        /// Queries the F4100 for all possible locations within a specificed Branch
        /// </summary>
        private void LocationMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LocationMasterTable.ResetAll();
            _LocationMasterTable.LMMCU = _BranchPlant;
            _LocationMasterTable.Fetch();
        }

        /// <summary>
        /// Simply updates the user's selected row, and moves on
        /// </summary>
        private void LocationMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LocationMasterTable.UpdateCurrentRow();
            CollectLocation_Enter();
        }

        #endregion

        #region ItemLocationList (F41021)

        /// <summary>
        /// Queries the F41021 for record depending on the Inquiry Type
        /// </summary>
        private void ItemLocationList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_InquiryType == "I") //Inquiring by 'I'
            {
                _ItemLocationTable.RetrieveItemLocationRecords(_BranchPlant, _ShortItemNumber, _ShowZeroOnHand);
            }
            else
            {
                _ItemLocationTable.RetrieveItemsInLocation(_BranchPlant, _Location, _ShowZeroOnHand);
            }
        }

        /// <summary>
        /// Updates thes information based on the user's selection, then retrieves data
        /// </summary>
        private void ItemLocationList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemLocationTable.UpdateCurrentRow();

            _ShortItemNumber = _ItemLocationTable.LIITM;
            _Location = _ItemLocationTable.LILOCN;
            _LotNumber = _ItemLocationTable.LILOTN;

            LoadData();
        }

        #endregion

        #region DisplayItemInformation

        /// <summary>
        /// When user presses ENTER, return to either the item number or the location collection screen
        /// depending on the workflow options
        /// (this event is also called when the Back key is pressed)
        /// </summary>
        private void DisplayItemInformation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ClearScreen();

            if (_InquiryType == "I")
            {
                RestartLoop(_CollectItemNumber);
            }
            else
            {
                RestartLoop(_CollectLocation);
            }
        }

        #endregion

        #region LoadData

        // load the detail information screen
        private void LoadData()
        {
            if (_ItemMasterTable.FetchItem(_ItemLocationTable.LIITM))
            {
                switch (_WarehouseConstants._DesignatedPrimaryItemNumber)
                {
                    case PrimaryItemNumberDesignations.ShortNumber:
                        UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMITM.ToString());
                        break;
                    case PrimaryItemNumberDesignations.LongNumber:
                        UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMLITM);
                        break;
                    case PrimaryItemNumberDesignations.ThirdNumber:
                        UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMAITM);
                        break;
                }

                _LocationMasterTable.ValidateLocation(_BranchPlant, _Location);
                UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _LocationMasterTable.sscnlocn);

                // Initialize dual qty and UOM data to blanks in case no conversion is found.
                UserSession.ScreenHandler.ScreenMap.Add("DualQtyOH", "");
                UserSession.ScreenHandler.ScreenMap.Add("DualUOMOH", "");
                UserSession.ScreenHandler.ScreenMap.Add("DualQtyAvailable", "");
                UserSession.ScreenHandler.ScreenMap.Add("DualUOMAvailable", "");

                if (_ItemLocationTable.RetrieveSpecificRecord(_BranchPlantTable.MCMCU, _ShortItemNumber, _Location, _LotNumber))
                {
                    string sQOH = _ItemLocationTable.LIPQOH.ToString();
                    double dPQOH = _ItemLocationTable.LIPQOH;
                    double dQtyAva = _ItemLocationTable.Availability;

                    UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _LocationMasterTable.sscnlocn);
                    UserSession.ScreenHandler.ScreenMap.Add("ONHAND", sQOH);
                    UserSession.ScreenHandler.ScreenMap.Add("AVAILABLE", dQtyAva.ToString());

                    #region Dual Unit of Measure check

                    double dDualQtyOH = 0;
                    double dDualQtyAva = 0;
                    string sDualUOM = string.Empty;
                    double dConversionFactor = 0;

                    // Is there a dual UOM value to work with?
                    if ((sDualUOM != null) && (sDualUOM.Trim() != string.Empty))
                    {
                        // Can we find a conversion to the dual UOM?
                        if (sDualUOM == _ItemMasterTable.IMUOM1)
                        {
                            dConversionFactor = 1;
                        }
                        else //The primary UOM is not the same as the dual UOM.
                        {
                            dConversionFactor = _UnitOfMeasureConversionTable1.ConvertToPrimaryUOM(_BranchPlant, _ShortItemNumber, _ItemMasterTable.IMUOM1, sDualUOM);
                        }

                        if (dConversionFactor == 0) // No conversion factor.  Try again with the defaults table
                        {
                            dConversionFactor = _UnitOfMeasureConversionTable2.ConvertToPrimaryUOM(_ItemMasterTable.IMUOM1, sDualUOM);
                        }

                        if (dConversionFactor == 0) //A conversion factor was not retrieved
                        {
                            // No.  Leave screen maps with empty default values.
                            Log.WriteLine(1, "Dual Unit of Measure conversion factor to convert from " + _ItemMasterTable.IMUOM1 + " to " + sDualUOM + " is zero.");
                        }
                        else
                        {
                            dDualQtyOH = dPQOH * dConversionFactor;
                            UserSession.ScreenHandler.ScreenMap.Add("DualQtyOH", dDualQtyOH.ToString());
                            UserSession.ScreenHandler.ScreenMap.Add("DualUOMOH", sDualUOM);

                            dDualQtyAva = dQtyAva * dConversionFactor;
                            UserSession.ScreenHandler.ScreenMap.Add("DualQtyAvailable", dDualQtyAva.ToString());
                            UserSession.ScreenHandler.ScreenMap.Add("DualUOMAvailable", sDualUOM);
                        }
                    }
                    else
                    {
                        Log.WriteLine(1, "Dual Unit of Measure for this item is either null or blank.");
                    }

                    #endregion
                }
                else
                {
                    string sQOH = "0";
                    double dQtyAva = 0;

                    UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _LocationMasterTable.sscnlocn);
                    UserSession.ScreenHandler.ScreenMap.Add("ONHAND", sQOH);
                    UserSession.ScreenHandler.ScreenMap.Add("AVAILABLE", dQtyAva.ToString());
                }
            }
            else
            {
                throw new RFS_Exception("ITEMNUMBER");
            }
        }

        #endregion

        #region ClearScreen

        private void ClearScreen()
        {
            UserSession.ScreenHandler.ScreenMap.Clear("ITEMNUMBER");
            UserSession.ScreenHandler.ScreenMap.Clear("LOCATION");
            UserSession.ScreenHandler.ScreenMap.Clear("LOTNUMBER");
            UserSession.ScreenHandler.ScreenMap.Clear("ONHAND");
            UserSession.ScreenHandler.ScreenMap.Clear("AVAILABLE");
            UserSession.ScreenHandler.BarcodeParseSystem.Reset(); //Clear the BarcodeParseSystem scanned items hash table
        }

        #endregion

        #endregion
    }
}