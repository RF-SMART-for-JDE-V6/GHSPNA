using System;
using ICS.Base.Wireless.JDE.BusinessConnector;
using ICS.Base.Wireless.JDE.BusinessConnector.Validators;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using ICS.Database.TableObjects.JDE;
using ICS.Stoneridge.Wireless.JDE.Utilities.Printing;
using ICS.Stoneridge.Wireless.RFS.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    public class LabelPrintConnector : BaseConnector
    {
        #region Class data

        #region Printing

        #region StoneridgePrinting

        protected StoneridgePrinting _StoneridgePrinting;

        public virtual StoneridgePrinting StoneridgePrinting
        {
            get
            {
                if (_StoneridgePrinting == null)
                {
                    _StoneridgePrinting = new StoneridgePrinting(UserSession);

                    _StoneridgePrinting.PrintXrefPatterns.Reset();
                    _StoneridgePrinting.PrintXrefPatterns.Add((PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                                                               PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.Item | PrintXrefType.Location));
                    _StoneridgePrinting.PrintXrefPatterns.Add((PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                                                               PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.Item | PrintXrefType.BlankLocation));
                    _StoneridgePrinting.PrintXrefPatterns.Add((PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                                                               PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.BlankItem | PrintXrefType.BlankLocation));
                    _StoneridgePrinting.PrintXrefPatterns.Add((PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                                                               PrintXrefType.ContainerType | PrintXrefType.BlankCustomer | PrintXrefType.BlankItem | PrintXrefType.BlankLocation));
                }

                return _StoneridgePrinting;
            }
        }

        #endregion

        #region GenCartonPrintData

        protected GenCartonPrintData _GenCartonPrintData;

        public virtual GenCartonPrintData GenCartonPrintData
        {
            get
            {
                if (_GenCartonPrintData == null)
                {
                    _GenCartonPrintData = new GenCartonPrintData(DataAccess);
                    _GenCartonPrintData.VariableNamePrefix = "ASR_";
                }

                return _GenCartonPrintData;
            }
        }

        #endregion

        #endregion>>

        #region Collected data

        public virtual string BranchPlant { get; protected set; }

        public virtual string BranchDescription { get; protected set; }

        public virtual string PrinterAddress { get; protected set; }

        public virtual string PrinterName { get; protected set; }

        public virtual int ContainerType { get; protected set; }

        #endregion

        #region ERPI

        #endregion

        #region Tables

        private F0006 tableBranchPlant;

        public F0006 _BranchPlantTable
        {
            get { return tableBranchPlant ?? (tableBranchPlant = new F0006(DataAccess, "")); }
        }

        #endregion

        #region Workflow options

        #endregion

        #region Base Printer Validator

        private PrintValidator _validatorPrinter;

        public PrintValidator PrinterValidator
        {
            get { return _validatorPrinter ?? (_validatorPrinter = new PrintValidator(Workflow)); }
        }

        #endregion

        #endregion

        #region Constructors and overrides

        public LabelPrintConnector(RFSWorkflowDesigner workflow) :
            base(workflow)
        {
        }

        public LabelPrintConnector() : base()
        {
        }

        #endregion

        #region Validators

        private BranchValidator _validatorBranch;

        public BranchValidator BranchValidator
        {
            get { return _validatorBranch ?? (_validatorBranch = new BranchValidator(Workflow)); }
        }

        #endregion

        #region Container Type

        public void ValidateConainerType(string ContainerTypeInput)
        {
            switch (ContainerTypeInput)
            {
                case "R":
                    ContainerType = 1;
                    ScreenMap.Add("ContainerType", "Returnable");
                    break;
                case "E":
                    ContainerType = 0;
                    ScreenMap.Add("ContainerType", "Expendable");
                    break;
                default:
                    throw new RFS_Exception("CONTAINER");
            }
        }

        #endregion

        #region Printer

        public string LoadDefaultPrinter()
        {
            StoneridgePrinting.ResetAll();
            Log.Debug($"<***>BranchPlant {BranchPlant}");
            StoneridgePrinting.Branch = BranchPlant.Trim();
            if (WFOptions.RetrieveOptionValue("LabelType") == "I")
            {
                StoneridgePrinting.ContainerType = "C";
            }
            else
            {
                StoneridgePrinting.ContainerType = "R";
            }

            Log.Debug($"<***> StoneridgePrinting.ContainerType = {StoneridgePrinting.ContainerType}");
            //Below we check if the PrinterName is blank, if so, it means no records were found for the 
            //ContainerType, so try another (we use "PrinterName" as once we have a valid value it stores
            //it and doesn't try to keep requirying like DefaultPrinter does)
            if (String.IsNullOrWhiteSpace(StoneridgePrinting.PrinterName))
            {
                StoneridgePrinting.ContainerType = "X";
            }

            if (String.IsNullOrWhiteSpace(StoneridgePrinting.PrinterName))
            {
                StoneridgePrinting.ContainerType = "C";
            }

            if (!String.IsNullOrWhiteSpace(StoneridgePrinting.PrinterName))
            {
                PrinterName = StoneridgePrinting.PrinterName;
                PrinterAddress = StoneridgePrinting.PrinterAddress;
                return StoneridgePrinting.PrinterName;
            }
            else
            {
                Workflow.Error(StoneridgePrinting.ErrorMessageVocabulary);
                return "";
            }
        }

        public void SetPrinter(string _PrinterName)
        {
            if (StoneridgePrinting.ValidatePrinter(_PrinterName))
            {
                PrinterName = StoneridgePrinting.PrinterName;
                PrinterAddress = StoneridgePrinting.PrinterAddress;
            }
            else
            {
                throw new RFS_Exception(StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        public void ValidatePrinter(string PrinterInput)
        {
            if (StoneridgePrinting.ValidatePrinter(PrinterInput))
            {
                PrinterName = StoneridgePrinting.PrinterName;
                PrinterAddress = StoneridgePrinting.PrinterAddress;
                ScreenMap.Add("PrinterName", PrinterName.Trim());
            }
            else
            {
                throw new RFS_Exception(StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        public void LoadPrinterList(GenericList ListPrinters)
        {
            //First hooks up the screens to the proper TableObject, and then query for them (note that all PrintXref
            //parameters have already been setup in the default printer func)
            PrinterValidator.PrinterListLoad(ListPrinters);
            StoneridgePrinting.RetrievePrinters();
        }

        #endregion

        #region Input validation

        public void ValidateBranchPlant(string BranchPlantInput)
        {
            BranchValidator.BranchValidate(BranchPlantInput);

            BranchPlant = BranchValidator.BranchPlant;
            BranchDescription = BranchValidator.BranchDescription;

            ScreenMap.Add("BranchPlant", BranchPlant);
            ScreenMap.Add("BranchDescription", BranchDescription);
        }

        #endregion
    }
}