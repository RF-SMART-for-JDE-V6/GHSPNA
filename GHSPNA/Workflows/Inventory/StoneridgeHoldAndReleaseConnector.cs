namespace ICS.Stoneridge.Wireless.JDE.Workflow.Inventory
{
    #region Usings

    using System;
    using ICS.Base.Wireless.JDE.BusinessConnector;
    using ICS.Base.Wireless.JDE.BusinessConnector.Validators;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;
    using ICS.Database.TableObjects.JDE;
    using ICS.ERPI.JDE;

    //using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

    #endregion

    public class StoneridgeHoldAndReleaseConnector : BaseConnector
    {
        #region Class Data

        #region Collected Data

        public virtual string BranchPlant { get; protected set; }

        public virtual string BranchDescription { get; protected set; }

        public virtual string Explanation { get; protected set; }

        public virtual string ReasonCode { get; protected set; }

        public virtual string PrimaryItemNumber { get; protected set; }

        public virtual double ShortItemNumber { get; protected set; }

        public virtual string LongItemNumber { get; protected set; }

        public virtual string ThirdItemNumber { get; protected set; }

        public virtual string ItemDescription1 { get; protected set; }

        public virtual string ItemDescription2 { get; protected set; }

        public virtual string ItemDimensionGroup { get; protected set; }

        public virtual string ItemLotProcessingType { get; protected set; }

        public virtual string ItemPrimaryUOM { get; protected set; }

        public virtual string LotSerialNumber { get; protected set; }

        public virtual string LotSerialGrade { get; protected set; }

        public virtual double LotSerialPotency { get; protected set; }

        public virtual DateTime? LotSerialExpirationDate { get; protected set; }

        public virtual string FromLocation { get; protected set; }

        public virtual string FormattedFromLocation { get; protected set; }

        public virtual string FromLocationPutawayZone { get; protected set; }

        public virtual string FromLocationPickZone { get; protected set; }

        public virtual string FromLocationReplenishZone { get; protected set; }

        public virtual string FromLocationDimensionGroup { get; protected set; }

        public virtual string ToLocation { get; protected set; }

        public virtual string FormattedToLocation { get; protected set; }

        public virtual string ToLocationPutawayZone { get; protected set; }

        public virtual string ToLocationPickZone { get; protected set; }

        public virtual string ToLocationReplenishZone { get; protected set; }

        public virtual string ToLocationDimensionGroup { get; protected set; }

        public virtual double Quantity { get; protected set; }

        public virtual string UnitOfMeasure { get; protected set; }

        public virtual int NumberOfLabels { get; protected set; }

        public virtual string PrinterAddress { get; protected set; }

        public virtual string PrinterName { get; protected set; }

        #endregion

        #region ERPI

        private ICS.ERPI.JDE.InvItemReclass ItemReclass_Tx = new InvItemReclass();

        #endregion

        #region Tables

        #region User Defined Code Values (F0005) JDE

        // Private Table Object
        private F0005 _tableJDEUserDefinedCodeValues;

        // Public Table Object (Generate New If NULL)
        public F0005 JDEUserDefinedCodeValuesTable
        {
            get { return _tableJDEUserDefinedCodeValues ?? (_tableJDEUserDefinedCodeValues = new F0005(DataAccess, "")); }
        }

        #endregion

        #region Location Master (F4100) JDE

        // Private Table Object
        private F4100 _tableJDELocationMaster;

        // Public Table Object (Generate New If NULL)
        public F4100 JDELocationMasterTable
        {
            get { return _tableJDELocationMaster ?? (_tableJDELocationMaster = new F4100(DataAccess, "")); }
        }

        #endregion

        #region Inventory Constants (F41001) JDE

        // Private Table Object
        private F41001 _tableJDEInventoryConstants;

        // Public Table Object (Generate New If NULL)
        public F41001 JDEInventoryConstantsTable
        {
            get { return _tableJDEInventoryConstants ?? (_tableJDEInventoryConstants = new F41001(DataAccess, "")); }
        }

        #endregion

        #region Item Units Of Measure Conversion Factors (F41002) JDE

        // Private Table Object
        private F41002 _tableJDEItemUnitsOfMeasureConversionFactors;

        // Public Table Object (Generate New If NULL)
        public F41002 JDEItemUnitsOfMeasureConversionFactorsTable
        {
            get { return _tableJDEItemUnitsOfMeasureConversionFactors ?? (_tableJDEItemUnitsOfMeasureConversionFactors = new F41002(DataAccess, "")); }
        }

        #endregion

        #region Unit Of Measure Standard Conversion (F41003) JDE

        // Private Table Object
        private F41003 _tableJDEUnitOfMeasureStandardConversion;

        // Public Table Object (Generate New If NULL)
        public F41003 JDEUnitOfMeasureStandardConversionTable
        {
            get { return _tableJDEUnitOfMeasureStandardConversion ?? (_tableJDEUnitOfMeasureStandardConversion = new F41003(DataAccess, "")); }
        }

        #endregion

        #region Item Master (F4101) JDE

        // Private Table Object
        private F4101 _tableJDEItemMaster;

        // Public Table Object (Generate New If NULL)
        public F4101 JDEItemMasterTable
        {
            get { return _tableJDEItemMaster ?? (_tableJDEItemMaster = new F4101(DataAccess, "")); }
        }

        #endregion

        #region Item Branch File (F4102) JDE

        // Private Table Object
        private F4102 _tableJDEItemBranchFile;

        // Public Table Object (Generate New If NULL)
        public F4102 JDEItemBranchFileTable
        {
            get { return _tableJDEItemBranchFile ?? (_tableJDEItemBranchFile = new F4102(DataAccess, "")); }
        }

        #endregion

        #region Item Location File (F41021) JDE

        // Private Table Object
        private F41021 _tableJDEItemLocationFile;

        // Public Table Object (Generate New If NULL)
        public F41021 JDEItemLocationFileTable
        {
            get { return _tableJDEItemLocationFile ?? (_tableJDEItemLocationFile = new F41021(DataAccess, "")); }
        }

        #endregion

        #region Lot Master (F4108) JDE

        // Private Table Object
        private F4108 _tableJDELotMaster;

        // Public Table Object (Generate New If NULL)
        public F4108 JDELotMasterTable
        {
            get { return _tableJDELotMaster ?? (_tableJDELotMaster = new F4108(DataAccess, "")); }
        }

        #endregion

        #endregion

        #region Workflow Options

        #region Allow Blank Location

        // Private Variable
        private bool? _wfoAllowBlankLocation;

        // Public Variable (Generate New If NULL)
        public bool AllowBlankLocation
        {
            get { return (bool) (_wfoAllowBlankLocation ?? (_wfoAllowBlankLocation = WFOptions.GetYesNoWFOptionValue("AllowBlankLocation", true))); }
        }

        #endregion

        #region Allow Blank Reason Code

        // Private Variable
        private bool? _wfoAllowBlankReasonCode;

        // Public Variable (Generate New If NULL)
        public bool AllowBlankReasonCode
        {
            get { return (bool) (_wfoAllowBlankReasonCode ?? (_wfoAllowBlankReasonCode = WFOptions.GetYesNoWFOptionValue("AllowBlankReasonCode", true))); }
        }

        #endregion

        #region Allow Unit Of Measure Override

        // Private Variable
        private bool? _wfoAllowUnitOfMeasureOverride;

        // Public Variable (Generate New If NULL)
        public bool AllowUnitOfMeasureOverride
        {
            get { return (bool) (_wfoAllowUnitOfMeasureOverride ?? (_wfoAllowUnitOfMeasureOverride = WFOptions.GetYesNoWFOptionValue("AllowUOMOverride", true))); }
        }

        #endregion

        #region Hold / Release Mode

        // Private Variable
        private int? _wfoHoldReleaseMode;

        // Enumerated Type Definition
        [Flags]
        public enum HoldReleaseModeDesignator
        {
            None = 0,

            Hold = 1,

            Release = 2
        }

        // Public Variable (Generate New If NULL)
        public HoldReleaseModeDesignator HoldReleaseMode
        {
            get
            {
                if (_wfoHoldReleaseMode == null)
                {
                    switch (WFOptions.RetrieveOptionValue("HoldRelease").Trim())
                    {
                        case "H":
                            _wfoHoldReleaseMode = (int) HoldReleaseModeDesignator.Hold;
                            break;
                        case "R":
                            _wfoHoldReleaseMode = (int) HoldReleaseModeDesignator.Release;
                            break;
                        default:
                            _wfoHoldReleaseMode = (int) HoldReleaseModeDesignator.None;
                            break;
                    }
                }

                return (HoldReleaseModeDesignator) (int) _wfoHoldReleaseMode;
            }
        }

        #endregion

        #region Hold Code

        // Private WFO variable
        private string _wfoHoldCode;

        // Public WFO variable (Generate New If NULL)
        public string HoldCode
        {
            get { return _wfoHoldCode ?? (_wfoHoldCode = WFOptions.RetrieveOptionValue("HoldCode")); }
        }

        #endregion

        #region Availability Check

        // Private Variable
        private int? _wfoAvailabilityCheck;

        // Enumerated Type Definition
        [Flags]
        public enum AvailabilityCheckDesignator
        {
            None = 0,

            OnHand = 1,

            Available = 2
        }

        // Public Variable (Generate New If NULL)
        public AvailabilityCheckDesignator AvailabilityCheck
        {
            get
            {
                if (_wfoAvailabilityCheck == null)
                {
                    switch (WFOptions.RetrieveOptionValue("AvailabilityCheck").Trim().ToUpper())
                    {
                        case "O":
                            _wfoAvailabilityCheck = (int) AvailabilityCheckDesignator.OnHand;
                            break;
                        case "A":
                            _wfoAvailabilityCheck = (int) AvailabilityCheckDesignator.Available;
                            break;
                        default:
                            _wfoAvailabilityCheck = (int) AvailabilityCheckDesignator.None;
                            break;
                    }
                }

                return (AvailabilityCheckDesignator) (int) _wfoAvailabilityCheck;
            }
        }

        #endregion

        #region Default Unit Of Measure

        // Private WFO variable
        private string _wfoDefaultUnitOfMeasure;

        // Public WFO variable (Generate New If NULL)
        public string DefaultUnitOfMeasure
        {
            get { return _wfoDefaultUnitOfMeasure ?? (_wfoDefaultUnitOfMeasure = WFOptions.RetrieveOptionValue("DefaultUOM")); }
        }

        #endregion

        #region Drop File Path

        // Private WFO variable
        private string _wfoDropFilePath;

        // Public WFO variable (Generate New If NULL)
        public string DropFilePath
        {
            get { return _wfoDropFilePath ?? (_wfoDropFilePath = WFOptions.RetrieveOptionValue("DropFilePath")); }
        }

        #endregion

        #region ERP Update Version

        // Private WFO variable
        private string _wfoERPUpdateVersion;

        // Public WFO variable (Generate New If NULL)
        public string ERPUpdateVersion
        {
            get { return _wfoERPUpdateVersion ?? (_wfoERPUpdateVersion = WFOptions.RetrieveOptionValue("ERPUpdateVersion")); }
        }

        #endregion

        #region Max Labels

        // Private WFO variable
        private int? _wfoMaxLabels;

        // Public WFO variable (Generate New If NULL)
        public int MaxLabels
        {
            get { return (int) (_wfoMaxLabels ?? (_wfoMaxLabels = WFOptions.GetInt32WFOptionValue("MaxLabels"))); }
        }

        #endregion

        #region Sync Transaction

        // Private Variable
        private bool? _wfoSyncTransaction;

        // Public Variable (Generate New If NULL)
        public bool SyncTransaction
        {
            get { return (bool) (_wfoSyncTransaction ?? (_wfoSyncTransaction = WFOptions.GetYesNoWFOptionValue("SyncTransaction", true))); }
        }

        #endregion

        #endregion

        #region Validators

        #region Base Branch Validator

        private BranchValidator _validatorBranch;

        public BranchValidator BranchValidator
        {
            get { return _validatorBranch ?? (_validatorBranch = new BranchValidator(Workflow)); }
        }

        #endregion

        #region Base Printer Validator

        private PrintValidator _validatorPrinter;

        public PrintValidator PrinterValidator
        {
            get { return _validatorPrinter ?? (_validatorPrinter = new PrintValidator(Workflow)); }
        }

        #endregion

        #endregion

        #endregion

        #region Constructors and Overrides

        public StoneridgeHoldAndReleaseConnector(RFSWorkflowDesigner workflow) : base(workflow)
        {
        }

        public StoneridgeHoldAndReleaseConnector() : base()
        {
        }

        #endregion

        #region Input Validation

        #region Branch Plant

        public void ValidateBranchPlant(string BranchPlantInput)
        {
            BranchValidator.BranchValidate(BranchPlantInput);

            BranchPlant = BranchValidator.BranchPlant;
            BranchDescription = BranchValidator.BranchDescription;

            ScreenMap.Add("BranchPlant", BranchPlant);
            ScreenMap.Add("BranchDescription", BranchDescription);
        }

        public void LoadBranchPlantList(GenericList BranchPlantList)
        {
            BranchValidator.BranchListLoad(BranchPlantList);
        }

        #endregion

        #region Explanation

        public void ValidateExplanation(string ExplanationInput)
        {
            Explanation = ExplanationInput;

            ScreenMap.Add("Explanation", Explanation);
        }

        #endregion

        #region Reason Code

        public void ValidateReasonCode(string ReasonCodeInput)
        {
            if (JDEUserDefinedCodeValuesTable.ValidateUDC("42  ", "RC", ReasonCodeInput))
            {
                if ((JDEUserDefinedCodeValuesTable.DRKY.Trim().Length == 0 && AllowBlankReasonCode) || JDEUserDefinedCodeValuesTable.DRKY.Trim().Length > 0)
                {
                    ReasonCode = JDEUserDefinedCodeValuesTable.DRKY;

                    ScreenMap.Add("ReasonCode", ReasonCode);
                }
                else
                {
                    throw new RFS_Exception("Blanks Not Allowed");
                }
            }
            else
            {
                throw new RFS_Exception("Reason Code");
            }
        }

        public void LoadReasonCodeList(GenericList ReasonCodeList)
        {
            ReasonCodeList.Table = JDEUserDefinedCodeValuesTable;
            JDEUserDefinedCodeValuesTable.FetchTypeOfHours("42  ", "RC");
        }

        #endregion

        #region Item Number

        public void ValidateItemNumber(string ItemNumberInput)
        {
            JDEInventoryConstantsTable.DeterminePrimaryItem(BranchPlant);
            if (!string.IsNullOrWhiteSpace(JDEItemMasterTable.RetrieveItem(BranchPlant, ItemNumberInput, JDEInventoryConstantsTable._DesignatedPrimaryItemNumber.ToString())))
            {
                if (JDEItemBranchFileTable.ValidateItemBranch(BranchPlant.Trim(), JDEItemMasterTable.IMITM))
                {
                    PrimaryItemNumber = JDEItemMasterTable.PrimaryItemNumber;
                    ShortItemNumber = JDEItemMasterTable.IMITM;
                    LongItemNumber = JDEItemMasterTable.IMLITM;
                    ThirdItemNumber = JDEItemMasterTable.IMAITM;
                    ItemDescription1 = JDEItemMasterTable.IMDSC1;
                    ItemDescription2 = JDEItemMasterTable.IMDSC2;
                    ItemPrimaryUOM = JDEItemMasterTable.IMUOM1;
                    ItemDimensionGroup = JDEItemBranchFileTable.IBPRP6;
                    ItemLotProcessingType = JDEItemBranchFileTable.IBSRCE;

                    ScreenMap.Add("ItemNumber", PrimaryItemNumber);
                    ScreenMap.Add("ItemDescription1", ItemDescription1);
                    ScreenMap.Add("ItemDescription2", ItemDescription2);
                }
                else
                {
                    throw new RFS_Exception("Invalid Item Branch");
                }
            }
            else
            {
                throw new RFS_Exception("Invalid Item Number");
            }
        }

        public void LoadItemNumberList(GenericList ItemNumberList)
        {
            ItemNumberList.Table = JDEItemMasterTable;
            JDEItemMasterTable.ShowItemsInBranch(BranchPlant, null, new string[] { });
        }

        #endregion

        #region Lot / Serial Number

        public void ValidateLotSerialNumber(string LotSerialNumberInput)
        {
            if (JDELotMasterTable.RetrieveLotMaster(BranchPlant, ShortItemNumber, LotSerialNumberInput))
            {
                LotSerialNumber = JDELotMasterTable.IOLOTN;
                LotSerialGrade = JDELotMasterTable.IOLOTG;
                LotSerialPotency = JDELotMasterTable.IOLOTP;
                LotSerialExpirationDate = DateTimeUtils.JDEJulian(JDELotMasterTable.IOMMEJ);

                ScreenMap.Add("LotSerialNumber", LotSerialNumber);
            }
            else
            {
                throw new RFS_Exception("LotSerialNumber");
            }
        }

        #endregion

        #region From Location

        public void ValidateFromLocation(string FromLocationInput)
        {
            if (AllowBlankLocation || FromLocationInput.Trim().Length > 0)
            {
                if (JDELocationMasterTable.ValidateLocation(BranchPlant.Trim(), FromLocationInput))
                {
                    if (0 < ItemLotProcessingType.Trim().Length)
                    {
                        if ((0 <= Convert.ToInt16(ItemLotProcessingType)) && (Convert.ToInt16(ItemLotProcessingType) <= Convert.ToInt16(LotSerialDesignations.LotRequired)))
                        {
                            if (JDEItemLocationFileTable.RetrieveSpecificRecord(BranchPlant, ShortItemNumber, JDELocationMasterTable.LMLOCN, LotSerialNumber))
                            {
                                FromLocation = JDELocationMasterTable.stbllocn;
                                FormattedFromLocation = JDELocationMasterTable.sscnlocn;
                                FromLocationPutawayZone = JDELocationMasterTable.LMPZON;
                                FromLocationPickZone = JDELocationMasterTable.LMKZON;
                                FromLocationReplenishZone = JDELocationMasterTable.LMZONR;
                                FromLocationDimensionGroup = JDELocationMasterTable.LMSTY1;

                                ScreenMap.Add("FromLocation", FormattedFromLocation);
                            }
                            else
                            {
                                throw new RFS_Exception("Lot Location");
                            }
                        }
                        else
                        {
                            if (JDEItemLocationFileTable.RetrieveSpecificRecord(BranchPlant, ShortItemNumber, JDELocationMasterTable.LMLOCN))
                            {
                                FromLocation = JDELocationMasterTable.stbllocn;
                                FormattedFromLocation = JDELocationMasterTable.sscnlocn;
                                FromLocationPutawayZone = JDELocationMasterTable.LMPZON;
                                FromLocationPickZone = JDELocationMasterTable.LMKZON;
                                FromLocationReplenishZone = JDELocationMasterTable.LMZONR;
                                FromLocationDimensionGroup = JDELocationMasterTable.LMSTY1;

                                ScreenMap.Add("FromLocation", FormattedFromLocation);
                            }
                            else
                            {
                                throw new RFS_Exception("Lot Location");
                            }
                        }
                    }
                    else
                    {
                        if (JDEItemLocationFileTable.RetrieveSpecificRecord(BranchPlant, ShortItemNumber, JDELocationMasterTable.LMLOCN))
                        {
                            FromLocation = JDELocationMasterTable.stbllocn;
                            FormattedFromLocation = JDELocationMasterTable.sscnlocn;
                            FromLocationPutawayZone = JDELocationMasterTable.LMPZON;
                            FromLocationPickZone = JDELocationMasterTable.LMKZON;
                            FromLocationReplenishZone = JDELocationMasterTable.LMZONR;
                            FromLocationDimensionGroup = JDELocationMasterTable.LMSTY1;

                            ScreenMap.Add("FromLocation", FormattedFromLocation);
                        }
                        else
                        {
                            throw new RFS_Exception("Lot Location");
                        }
                    }
                }
                else
                {
                    throw new RFS_Exception("Location");
                }
            }
            else
            {
                throw new RFS_Exception("Blanks Not Allowed");
            }
        }

        #endregion

        #region To Location

        public void ValidateToLocation(string ToLocationInput)
        {
            if (AllowBlankLocation || ToLocationInput.Trim().Length > 0)
            {
                if (JDELocationMasterTable.ValidateLocation(BranchPlant.Trim(), ToLocationInput))
                {
                    ToLocation = JDELocationMasterTable.stbllocn;
                    FormattedToLocation = JDELocationMasterTable.sscnlocn;
                    ToLocationPutawayZone = JDELocationMasterTable.LMPZON;
                    ToLocationPickZone = JDELocationMasterTable.LMKZON;
                    ToLocationReplenishZone = JDELocationMasterTable.LMZONR;
                    ToLocationDimensionGroup = JDELocationMasterTable.LMSTY1;

                    ScreenMap.Add("ToLocation", FormattedToLocation);
                }
                else
                {
                    throw new RFS_Exception("Location");
                }
            }
            else
            {
                throw new RFS_Exception("Blanks Not Allowed");
            }
        }

        #endregion

        #region Quantity

        public void ValidateQuantity(string QuantityInput)
        {
            double ParsedQuantity;

            string CalculatedQuantity = JDEItemLocationFileTable.CalcQty(QuantityInput.ToUpper());

            if (CalculatedQuantity != null)
            {
                if (double.TryParse(CalculatedQuantity, out ParsedQuantity))
                {
                    Quantity = ParsedQuantity;

                    ScreenMap.Add("Quantity", Quantity.ToString());
                }
                else
                {
                    throw new RFS_Exception("Not Numeric");
                }
            }
            else
            {
                throw new RFS_Exception("Invalid Quantity Expression");
            }
        }

        #endregion

        #region Unit Of Measure

        public string LoadDefaultUnitOfMeasure()
        {
            return JDEItemMasterTable.DefaultUOM(DefaultUnitOfMeasure);
        }

        public void ValidateUnitOfMeasure(string UnitOfMeasureInput)
        {
            double ConversionFactor = 0;
            double QuantityToReclass = 0;
            double QuantityAvailable = 0;

            if (AllowUnitOfMeasureOverride || UnitOfMeasureInput.Trim().ToUpper() == JDEItemMasterTable.DefaultUOM(DefaultUnitOfMeasure).Trim().ToUpper())
            {
                if (UnitOfMeasureInput != ItemPrimaryUOM)
                {
                    ConversionFactor = JDEItemUnitsOfMeasureConversionFactorsTable.ConvertToPrimaryUOM(BranchPlant, ShortItemNumber, UnitOfMeasureInput, ItemPrimaryUOM);

                    if (ConversionFactor == 0)
                    {
                        ConversionFactor = JDEUnitOfMeasureStandardConversionTable.ConvertToPrimaryUOM(UnitOfMeasureInput, ItemPrimaryUOM);
                    }
                }
                else
                {
                    ConversionFactor = 1;
                }

                if (ConversionFactor != 0)
                {
                    QuantityToReclass = Quantity * ConversionFactor;

                    if (0 < ItemLotProcessingType.Trim().Length)
                    {
                        if ((0 <= Convert.ToInt16(ItemLotProcessingType)) && (Convert.ToInt16(ItemLotProcessingType) <= Convert.ToInt16(LotSerialDesignations.LotRequired)))
                        {
                            if (JDEItemLocationFileTable.RetrieveSpecificRecord(BranchPlant, ShortItemNumber, FromLocation, LotSerialNumber))
                            {
                                switch (AvailabilityCheck)
                                {
                                    case AvailabilityCheckDesignator.Available:
                                        QuantityAvailable = Convert.ToDouble(JDEItemLocationFileTable.Availability);
                                        break;
                                    case AvailabilityCheckDesignator.OnHand:
                                        QuantityAvailable = JDEItemLocationFileTable.LIPQOH;
                                        break;
                                    case AvailabilityCheckDesignator.None:
                                        QuantityAvailable = QuantityToReclass;
                                        break;
                                }

                                if (QuantityToReclass <= QuantityAvailable)
                                {
                                    Quantity = QuantityToReclass;
                                    UnitOfMeasure = ItemPrimaryUOM;
                                }
                                else
                                {
                                    throw new RFS_Exception("Insufficient Quantity");
                                }
                            }
                            else
                            {
                                throw new RFS_Exception("Invalid Unit Of Measure");
                            }
                        }
                        else
                        {
                            if (JDEItemLocationFileTable.RetrieveSpecificRecord(BranchPlant, ShortItemNumber, FromLocation))
                            {
                                switch (AvailabilityCheck)
                                {
                                    case AvailabilityCheckDesignator.Available:
                                        QuantityAvailable = Convert.ToDouble(JDEItemLocationFileTable.Availability);
                                        break;
                                    case AvailabilityCheckDesignator.OnHand:
                                        QuantityAvailable = JDEItemLocationFileTable.LIPQOH;
                                        break;
                                    case AvailabilityCheckDesignator.None:
                                        QuantityAvailable = Quantity;
                                        break;
                                }

                                Log.Debug("QuantityToReclass = " + QuantityToReclass);
                                Log.Debug("QuantityAvailable = " + QuantityAvailable);
                                if (QuantityToReclass <= QuantityAvailable)
                                {
                                    Quantity = QuantityToReclass;
                                    UnitOfMeasure = ItemPrimaryUOM;
                                }
                                else
                                {
                                    throw new RFS_Exception("Insufficient Quantity");
                                }
                            }
                            else
                            {
                                throw new RFS_Exception("Invalid Unit Of Measure");
                            }
                        }
                    }
                    else
                    {
                        if (JDEItemLocationFileTable.RetrieveSpecificRecord(BranchPlant, ShortItemNumber, FromLocation))
                        {
                            switch (AvailabilityCheck)
                            {
                                case AvailabilityCheckDesignator.Available:
                                    QuantityAvailable = Convert.ToDouble(JDEItemLocationFileTable.Availability);
                                    break;
                                case AvailabilityCheckDesignator.OnHand:
                                    QuantityAvailable = JDEItemLocationFileTable.LIPQOH;
                                    break;
                                case AvailabilityCheckDesignator.None:
                                    QuantityAvailable = Quantity;
                                    break;
                            }

                            Log.Debug("QuantityToReclass = " + QuantityToReclass);
                            Log.Debug("QuantityAvailable = " + QuantityAvailable);
                            if (QuantityToReclass <= QuantityAvailable)
                            {
                                Quantity = QuantityToReclass;
                                UnitOfMeasure = ItemPrimaryUOM;
                            }
                            else
                            {
                                throw new RFS_Exception("Insufficient Quantity");
                            }
                        }
                        else
                        {
                            throw new RFS_Exception("Invalid Unit Of Measure");
                        }
                    }
                }
                else
                {
                    throw new RFS_Exception("Invalid Unit Of Measure");
                }
            }
            else
            {
                throw new RFS_Exception("Override Not Allowed");
            }
        }

        #endregion

        #region Number Of Labels

        public void ValidateNumberOfLabels(string NumberOfLabelsInput)
        {
            PrinterValidator._MaxLabels = MaxLabels;
            PrinterValidator.ValidateNumberOfLabels(NumberOfLabelsInput);

            NumberOfLabels = PrinterValidator._NumberOfLabels;

            ScreenMap.Add("NumberOfLabels", NumberOfLabels.ToString());
        }

        #endregion

        #region Printer

        public string LoadDefaultPrinter()
        {
            PrinterValidator.SetDefaultPrinter();
            return PrinterValidator._DefaultPrinter;
        }

        public void ValidatePrinter(string PrinterInput)
        {
            PrinterValidator.ValidatePrinter(PrinterInput);

            PrinterAddress = PrinterValidator._PrinterAddress;
            PrinterName = PrinterValidator._PrinterName;

            ScreenMap.Add("PrinterName", PrinterName);

            PrintBarCodeLabel();
        }

        public void LoadPrinterList(GenericList PrinterList)
        {
            PrinterValidator.PrinterListLoad(PrinterList);
        }

        #endregion

        #endregion

        #region Helper Methods

        #region Item Is Lot Controlled

        public bool ItemIsLotControlled()
        {
            if (ItemLotProcessingType.Trim().Length > 0)
            {
                if ((Convert.ToInt16(ItemLotProcessingType) >= 0) && (Convert.ToInt16(ItemLotProcessingType) <= (int) LotSerialDesignations.LotRequired))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Clear Lot Details

        public void ClearLotDetails()
        {
            LotSerialNumber = " ";
            LotSerialGrade = " ";
            LotSerialPotency = -1;
            LotSerialExpirationDate = null;

            ScreenMap.Add("LotSerialNumber", LotSerialNumber);
        }

        #endregion

        #region Send To ERP

        public override bool SendToERP()
        {
            Transactions.Add(ItemReclass_Tx);
            Transactions.Branch = BranchPlant.Trim().ToUpperInvariant();
            ItemReclass_Tx.Initialize();

            #region Set From Location Parameters

            ItemReclass_Tx.FromBranch = BranchPlant;
            ItemReclass_Tx.FromLocation = FromLocation;
            ItemReclass_Tx.FromLocnPutZone = FromLocationPutawayZone.Trim();
            ItemReclass_Tx.FromLocnPickZone = FromLocationPickZone.Trim();
            ItemReclass_Tx.FromLocnReplZone = FromLocationReplenishZone.Trim();
            ItemReclass_Tx.FromLocnDimGroup = FromLocationDimensionGroup.Trim();

            #endregion

            #region Set From Item Number Paramaters

            ItemReclass_Tx.FromPrimaryItemNumber = PrimaryItemNumber;
            ItemReclass_Tx.FromShortItemNumber = ShortItemNumber.ToString();
            ItemReclass_Tx.FromSecondItemNumber = LongItemNumber;
            ItemReclass_Tx.FromThirdItemNumber = ThirdItemNumber;
            ItemReclass_Tx.FromItemDescription1 = ItemDescription1;
            ItemReclass_Tx.FromItemDescription2 = ItemDescription2;
            ItemReclass_Tx.FromItemDimGroup = ItemDimensionGroup.Trim();
            ItemReclass_Tx.FromItemLotProcType = ItemLotProcessingType;

            #endregion

            #region Set From Lot Number Parameters

            ItemReclass_Tx.FromLot = LotSerialNumber.Trim();
            ItemReclass_Tx.FromLotStatus = " ";

            if (ItemLotProcessingType.Trim() == "")
            {
                ItemReclass_Tx.FromLotProcessType = " ";
            }
            else
            {
                ItemReclass_Tx.FromLotProcessType = ItemLotProcessingType;
            }

            if (LotSerialExpirationDate != null)
            {
                ItemReclass_Tx.FromExpDate_YYYYMMDD = ((DateTime) LotSerialExpirationDate).ToString("yyyyMMdd");
            }
            else
            {
                ItemReclass_Tx.FromExpDate_YYYYMMDD = "00000000";
            }

            #endregion

            #region Set To Lot Number Parameters

            ItemReclass_Tx.ToLot = LotSerialNumber.Trim();
            ItemReclass_Tx.ToLotGrade = LotSerialGrade;
            ItemReclass_Tx.ToLotPotency = LotSerialPotency.ToString();

            if (ItemLotProcessingType.Trim() == "")
            {
                ItemReclass_Tx.ToLotProcessType = " ";
            }
            else
            {
                ItemReclass_Tx.ToLotProcessType = ItemLotProcessingType;
            }

            if (LotSerialExpirationDate != null)
            {
                ItemReclass_Tx.ToLotExpDate_YYYYMMDD = ((DateTime) LotSerialExpirationDate).ToString("yyyyMMdd");
            }
            else
            {
                ItemReclass_Tx.ToLotExpDate_YYYYMMDD = "00000000";
            }

            if (HoldReleaseMode == HoldReleaseModeDesignator.Hold)
            {
                ItemReclass_Tx.ToLotStatus = HoldCode;
            }
            else
            {
                ItemReclass_Tx.ToLotStatus = " ";
            }

            #endregion

            #region Set To Item Number Parameters

            ItemReclass_Tx.ToPrimaryItemNumber = PrimaryItemNumber;
            ItemReclass_Tx.ToShortItemNumber = ShortItemNumber.ToString();
            ItemReclass_Tx.ToSecondItemNumber = LongItemNumber;
            ItemReclass_Tx.ToThirdItemNumber = ThirdItemNumber;
            ItemReclass_Tx.ToItemDescription1 = ItemDescription1;
            ItemReclass_Tx.ToItemDescription2 = ItemDescription2;
            ItemReclass_Tx.ToItemDimGroup = ItemDimensionGroup;
            ItemReclass_Tx.ToItemLotProcType = ItemLotProcessingType;

            #endregion

            #region Set To Location Paramaters

            ItemReclass_Tx.ToBranch = BranchPlant;
            ItemReclass_Tx.ToLocation = ToLocation;
            ItemReclass_Tx.ToLocnPutZone = ToLocationPutawayZone.Trim();
            ItemReclass_Tx.ToLocnPickZone = ToLocationPickZone.Trim();
            ItemReclass_Tx.ToLocnReplZone = ToLocationReplenishZone.Trim();
            ItemReclass_Tx.ToLocnDimGroup = ToLocationDimensionGroup.Trim();

            #endregion

            #region Set Explanation / Reason Code Parameters

            ItemReclass_Tx.ExplanationTransaction = Explanation;
            ItemReclass_Tx.ReasonCode = ReasonCode.Trim();

            #endregion

            #region Set Quantity / Unit Of Measure Parameter

            ItemReclass_Tx.Quantity = Quantity.ToString();
            ItemReclass_Tx.UOM = UnitOfMeasure;
            ItemReclass_Tx.SecondaryQuantity = "0";
            ItemReclass_Tx.SecondaryUOM = "";

            #endregion

            #region Set Additional Parameters

            ItemReclass_Tx.RFUserID = UserSession.CurrentUser.LoginName;
            ItemReclass_Tx.TransDate_YYYYMMDD = System.DateTime.Now.ToString("yyyyMMdd");
            ItemReclass_Tx.Version = ERPUpdateVersion.Trim();

            #endregion

            Transactions.Send(SyncTransaction ? "Y" : "N");
            Workflow.DisplayMessages();

            return !Transactions.Error;
        }

        #endregion

        #region Print BarCode Label

        public void PrintBarCodeLabel()
        {
            Workflow.PrintSystem.DropFilePathOverride = DropFilePath;
            Workflow.PrintSystem.PrinterAddress = PrinterAddress;
            Workflow.PrintSystem.PrinterName = PrinterName;
            Workflow.PrintSystem.Copies = NumberOfLabels;

            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ThirdItemNumber", ThirdItemNumber, false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ShortItemNumber", ShortItemNumber.ToString(), false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Description1", ItemDescription1, false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Descrition2", ItemDescription2, false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_SecondItemNumber", LongItemNumber, false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", LotSerialNumber, false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_TransactionQuantity", Quantity.ToString(), false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_UnitOfMeasure", UnitOfMeasure, false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LocationMasterTable", FormattedToLocation, false);
            Workflow.PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_PrimaryItemNumber", PrimaryItemNumber, false);

            Workflow.PrintSystem.ExtraPrintData.NewRow();

            if (!Workflow.PrintSystem.Print())
                Workflow.DisplayMessage(Workflow.PrintSystem.ErrorMessageVocabulary);
        }

        #endregion

        #endregion
    }
}