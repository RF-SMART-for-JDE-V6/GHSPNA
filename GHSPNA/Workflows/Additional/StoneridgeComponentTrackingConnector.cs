namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    #region Usings

    using ICS.Base.Wireless.JDE.BusinessConnector;
    using ICS.Base.Wireless.JDE.BusinessConnector.Validators;
    using ICS.Workflow.Base.WorkflowDesign.Components;
    using ICS.Database.TableObjects.JDE;

    #endregion

    public class StoneridgeComponentTrackingConnector : BaseConnector
    {
        #region Class data

        #region Collected data

        public virtual string BranchPlant { get; protected set; }

        public virtual string BranchDescription { get; protected set; }

        #endregion

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

        #region Validators

        private BranchValidator _validatorBranch;

        public BranchValidator BranchValidator
        {
            get { return _validatorBranch ?? (_validatorBranch = new BranchValidator(Workflow)); }
        }

        #endregion

        #region Constructors and overrides

        public StoneridgeComponentTrackingConnector(RFSWorkflowDesigner workflow) :
            base(workflow)
        {
        }

        public StoneridgeComponentTrackingConnector() : base()
        {
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