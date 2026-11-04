/*
    $Archive:   \\filer\rfsow\pvcsproj\RFSmart3\archives\Base\RTTC_Plugins\ICS\Workflow\Base\JDE\ShopFloor\StoneridgeMasterPallet.cs-arc  $
    $Revision: 20304 $
    $Author: sstroble@ICSNT $
    $Date: 2017-03-15 15:03:48 -0600 (Wed, 15 Mar 2017) $
    $Log:   \\filer\rfsow\pvcsproj\RFSmart3\archives\Base\RTTC_Plugins\ICS\Workflow\Base\JDE\ShopFloor\StoneridgeMasterPallet.cs-arc  $
 * 
 *    Rev 1.141   Feb 20 2009 10:48:34   dlomneck
 * 029019 - Assigned correct UOM variable prior to calling ProcessScrapUOMEnter(); to avoid workflow error when selecting from an F1 list.
 * 
 *    Rev 1.140   Dec 04 2008 11:25:06   ghripto
 * BE013638: DUOM project
 * 
 *    Rev 1.139   Jul 25 2008 15:30:54   cgray
 * BD027065 - Removed the extra setting of the Description1 print data field with the IMDSC2 as it overwrites the IMDSC1 data.
 * In order to maintain some level of backwards compatibility with Bartender, a Description2 was not added to take the IMDSC2
 * data.  Most likely, the ItemLabel plugin will be will used which spits out the F4101.  Therefore, the user could get the data
 * from there.
 * 
 *    Rev 1.138   Jun 19 2008 15:20:54   rgreene
 *  BD026578: WO Completions - Workflow Error 
 * 
 *    Rev 1.137   May 16 2008 16:10:38   rgreene
 *  BD025864: StoneridgeMasterPallet 
 * 
 *    Rev 1.136   07 Feb 2008 14:57:12   rgreene
 * BD024509: StoneridgeMasterPallets - CollectLicensePlate Screen 
 * 
 *    Rev 1.135   17 Dec 2007 15:26:10   rgreene
 *  BD023700: Wrong quantity updated and wrong lot number displayed 
 * 
 *    Rev 1.134   10 Dec 2007 14:55:28   rgreene
 * BE021727: JDEReceiptToContainer - CollectLotSerialExpDate 
 * 
 *    Rev 1.133   12 Nov 2007 11:27:44   rgreene
 * BE015634: Change the F0006_List to more meaningful information 
 * 
 *    Rev 1.132   09 Nov 2007 10:59:34   rgreene
 * BD023399: Problem with Unitime 
 * 
 *    Rev 1.131   07 Nov 2007 15:33:56   rgreene
 * BE014212: Expiration Date screen is not presented for serialized items (SRCE = 4,5,6) 
 * 
 *    Rev 1.130   16 Oct 2007 11:18:42   rgreene
 * BE022712: Generate Lot number in RFS for RV and WC 
 * 
 *    Rev 1.129   13 Sep 2007 14:25:26   rgreene
 * BD014194: W/O Comp, IBSRCE = 4 
 * 
 *    Rev 1.128   14 Aug 2007 11:10:52   rgreene
 * BD022274
 * 
 *    Rev 1.127   Aug 14 2007 10:56:50   cgray
 * BD022240 - Changed the OverCompleted method to always check the total transactional quantity against what is available
 * for the order in order to correctly choose the correct ERP Update Version based on the appropriate workflow option.
 * Also moved the transaction quantity conversion from Primary to Document UOM before the ERP Update Version determination
 * as that check should be in the correct UOM too.
 * 
 * General code cleanup and formatting.
 *
 *    Rev 1.126   16 Jul 2007 15:25:50   rgreene
 * BD021609: StoneridgeJobNumberLogOnOff - LPLogOff
 *
 *    Rev 1.125   13 Jul 2007 17:02:34   rgreene
 * BD021596: StoneridgeMasterPallet - LicensePlating
 *
 *    Rev 1.124   26 Jun 2007 16:53:20   rgreene
 * BD021359: JDE Work Orders - Component object model
 *
 *    Rev 1.123   Jun 12 2007 12:55:26   cgray
 * BD021240 - Added an assignment of the ERPUpdateVersion field within the OverComplete method when the workflow
 * option to allow over complete is set to 'Y' in order to ensure that it always has a value when the SendToERP method is called.
 *
 *    Rev 1.122   Jun 11 2007 15:41:18   cgray
 * BD021221 - Changed the KeyEvent event handler for the CollectLicensePlate screen to not check the LicensePlate to
 * see if it is empty or not in the event that its value is null.  Besides, the assignment of the UserInput to the LicensePlate
 * would always result in an empty string for KeyEvents.
 *
 *    Rev 1.121   20 Apr 2007 10:53:12   rgreene
 * BE020741: WO completion OW Versions
 *
 *    Rev 1.120   04 Apr 2007 16:46:16   rgreene
 * BD020596: Code Cleanup
 *
 *    Rev 1.119   Feb 15 2007 14:15:42   rgreene
 * BD019891: Ent 1 Workflows and CurrentRow
 *
 *    Rev 1.118   Feb 08 2007 14:50:58   rgreene
 * BD017932: Bairnco - Lot Grade and Potency missing in work order completion
 *
 *    Rev 1.117   Feb 06 2007 09:54:02   rgreene
 * BD017932: Bairnco - Lot Grade and Potency missing in work order completion
 *
 *    Rev 1.116   Jan 29 2007 17:14:02   rgreene
 * BD019658: W/O Completion
 *
 *    Rev 1.115   Dec 19 2006 11:58:40   rgreene
 * BD017932: Bairnco - Lot Grade and Potency missing in work order completion
 *
 *    Rev 1.114   Jul 28 2006 13:33:36   rgreene
 * BE015285: Shift code description
 *
 *    Rev 1.113   Jun 12 2006 13:51:18   pmonaco
 *  BE016606 - Update to .NET 2.0
 *
 *    Rev 1.112   May 22 2006 14:56:46   pmonaco
 * BD016013 - Workflows printer selection modified to use Print System validprinter method.  This checks to make sure the form matches the printer model before printing.
 *
 *    Rev 1.111   May 22 2006 12:24:40   cgray
 * BD015233 - Changed the ProcessLotNumberEnter method to call the F41021 table object's IsLotOnHold method to
 * determine if the lot entered is on hold based on the AllowedLotHoldCodes workflow option.  If the lot is on hold, the
 * user receives an error message stating such.
 *
 * - Also changed the CollectLot_ListEvent method to pass the array of allowed hold codes to the F41021 table object's
 *   RetrieveItemSerialRecords method to exclude held lots from the list.
 *
 *    Rev 1.110   May 19 2006 13:58:08   cgray
 * BD016313 - Changed the ScrapUOMEnter method to only go to the LicensePlateScreen if a Completed Quantity has been
 * entered and LicensePlating is enabled.
 *
 *    Rev 1.109   May 19 2006 13:37:00   cgray
 * BD016313 - Added the Collection of the License Plate to the Collect Scrap methods.
 *
 *    Rev 1.108   May 18 2006 11:49:16   cgray
 * Moved the IsLicensePlatingEnabled method to the Base workflow class so that all workflows may take advantage of it.
 *
 *    Rev 1.107   May 15 2006 12:37:58   cgray
 * BD016113 - Changed the OverComplete method to convert the transactional quantities to the Document (WAUOM) UOM to
 * ensure that the check is checking the quantities within the same UOM.  If the conversion cannot be performed, an
 * exception is thrown and picked up by the Screen object's FireEvent displaying an Inv Over-Comp Conv message.
 *
 * BD016056 - Changed the ToleranceCheck method to throw an
 * exception if the conversion from the Document UOM (WAUOM)
 * to te primary cannot be performed.  The exception is caught by  the Screen object's FireEvent displaying an Inv Tolerance Conv
 * message.
 *
 *    Rev 1.106   May 15 2006 10:57:40   cgray
 * BD016056 - Changed the workflow's tolerance check to check the document quantities and the transaction quantities in the
 * same UOM for a proper check.  The ToleranceCheck method converts the total transactional quantity to the document UOM
 * so the tolerance check is accurate.
 *
 * - Changed the name of the F41002 table object object field from _UnitOfMeasureConversionTable to _tblUOMConversion
 *   so it is smaller.
 *
 * - Changed the OverComplete method to fetch the AllowOverComplete workflow option instead of the
 *   OverComplete workflow option as it was changed to remain consistent with Yes/No workflow options defaulting to No.
 *
 * - Added the ToleranceCheck to the ProcessScrapUOM method since the Scrap quantity is included in the ToleranceCheck.
 *
 *    Rev 1.105   May 12 2006 14:54:24   cgray
 * BD015364 - Changed the workflow to use the stack to determine which screens to go back to; therefore, the back
 * events should be using the Application_BackEvent method, which goes back one screen.  This was done so that no
 * additional logic was necessary to determine which screen to return to if a list is used by multiple screens.
 *
 * Changed the CollectLocation_KeyEvent method to call the Show for the F4100Empty_List as opposed to Next.
 *
 *    Rev 1.104   May 12 2006 13:25:38   cgray
 * BD015270 - Changed the workflow to use the Input class's conversion methods, which has exception handling logic
 * embedded within them to prevent unexpected errors.
 *
 *    Rev 1.103   May 01 2006 11:41:44   cgray
 * BD015270 - Changed the workflow to set the _WorkOrderNumber by converting the e.UserInput to a double
 * within the CollectWorkOrderNumber enter event method.
 *
 *    Rev 1.102   Apr 28 2006 13:55:26   cgray
 * BD015281 - Mapped the Completed and Scrapped UOM entered by the user right after a conversion is found.  This is
 * used to be displayed on the necessary screens.
 *
 *    Rev 1.101   Apr 28 2006 10:24:22   cgray
 * BD015266 - Changed the ExceedsTolerance method to calculate the amount of Tolerance based on only the Ordered
 * Quantity.  Therefore, the user may only complete up to the Ordered quantity plus the Tolerance Quantity (Tolerance % *
 * Ordered quantity), or must complete at least the Ordered quantity minus the Tolerance Quantity.  The amount completed
 * is equal to the already completed quantity (WASOQS) plus the canceled quantity (WASOCN) plus the quantity entered during
 * the execution of the current transaction.
 *
 *    Rev 1.100   Apr 27 2006 16:46:14   cgray
 * BD015238 - Changed the workflow to use the Ceiling of the unconverted Completed Quantity to determine the default label
 * quantity.  This will ensure that decimal values are rounded up.
 *
 * - Removed the GetUOMConversion method as there is an exact method within the F41002 class, which is now being
 *   used.  By removing that method and calling the F41002 class's ConvertUnitOfMeasure method, the F41003 object was
 *   also removed as it is no longer needed.
 *
 * - Removed an unneeded F0006 object as there were two declared.
 *
 *    Rev 1.99   Apr 26 2006 16:38:20   cgray
 * BD015264 - The workflow had two RFS_PrinterDefinitions objects defined, and they kept getting switched around.
 * Removed one of the RFS_PrinterDefinitions objects leaving just one to prevent any confusion.
 *
 * - Added the following methods for encapsulation purposes:
 *   - CheckLotProcessType, which is used to check the IBSRCE field.  If the field is Lot controlled, it goes to the collect lot
 *     screen.  If  the field is Serial controlled, the CollectSerials method is called; otherwise, the transaction is sent to JDE.
 *   - CollectSerials, which is used to determine if the IBSRCE is a system generated serial or user entered serial.  If system
 *     generated, the transaction is sent to JDE; otherwise, the user is directed to the CollectSerialNumber screen.
 *
 * Note:  these were just for encapsulation purposes only.
 *
 *    Rev 1.98   Apr 25 2006 13:14:34   cgray
 * BD015262 - Changed the workflow to hold the quantity and UOM completed and quantity scrapped as entered by the user
 * in a separate field.  These are used to maintain the quantity and UOM before they are converted to the primary.  The
 * unconverted quantities are used to convert the quantity to the primary and store it in the converted quantity fields.  This is
 * necessary because if an error were thrown for the UOM entered, the quantity should not change when converting again
 * with a valid UOM.  The unconverted UOM is used for license plating.  The unconverted completed quantity is also used to
 * default the label quantity.
 *
 * - Removed the license plating logic within the Scrapped quantity and UOM because license plating should only occur
 *   for completed quantities.
 *
 * - Add the following methods:
 *
 *   GetNextScreenForCompUOM, which is used to determine the screen the user should go to next after a valid
 *   completed UOM is entered.  This was done because the same logic was duplicated for the ProcessUOMEnter and
 *   DisplayToleranceWarning methods.
 *
 *   GetUOMConversion, which is for nothing more than modularity.  The logic that was used for both the ProcessUOMEnter and
 *   ProcessScrapUOMEnter methods to determine the conversion factor was encapsulated into this new method, which returns the conversion
 *   factor.
 *
 * - Removed some unneeded returns and ResetAll calls to methods that already call ResetAll.
 *
 *    Rev 1.97   Apr 18 2006 15:56:44   cgray
 * BD015238 - Changed the workflow to never default a label quantity less than zero.  Usually the label quantity is defaulted
 * based on the number of items completed, but if a negative completion is done, the negative quantity is defaulted, which is
 * incorrect.
 *
 * BD015244 - Changed the workflow to verify that if the AllowUOMOverride is set to 'N', the user must enter the default UOM as determined by workflow option DefaultUOM.  This change has been made for both Completed and Scrapped UOM enter and list events.
 *
 * BD015262 - Changed the workflow to display an error message if the OverComplete workflow option is set to 'Y' and
 * the user completes more than is on the work order.
 *
 * Also changed all RetrieveOptionValue method calls to GetYesNoOptionValue.  This ensures that any options that
 * should only have a 'Y' or 'N' as a valid value but have something other than a 'Y' or 'N' throw an RFS_Exception, which will
 * display a message to the user.
 *
 *    Rev 1.96   Apr 13 2006 16:50:48   cgray
 * BE013524 - Removed the code that clears the _LicensePlate within the CollectLicensePlate_KeyEvent method so that a
 * licence plate number may be manually created.  This will set the processing type flag to "N" for new license plate and will
 * pass the one that needs to be created.
 *
 * BD015237 -  Changed the workflow to call the IsWorkOrderValid method within the F4801 to validate the Work
 * Order specified by the user taking into consideration the value of the AllowStatusCodes workflow option to ensure that the
 * Work Order's is in the proper status
 *
 * - Removed the workflow's RFS_WorkFlowOption object because the base workflow already has one that is initialized
 *   when the Start method is called.
 *
 * - Changed the method for the CollectSerialNumber Enter event from SerialNumber_EnterEvent to
 *   CollectSerialNumber_EnterEvent.  This follows the standard naming convention of the screen events, which is to name the
 *   event based on the object name, which in this case is CollectSerialNumber.
 *
 * - Changed the object name for the _LotSerialExpirationDate to _LotSerialExpDate as this closely matches the actual screen
 *   name.
 *
 * - Removed unnecessary uses of return in methods returning void
 *
 * - Removed unnecessary uses of the this keyword
 *
 * - General code cleanup and formatting
 *
 *    Rev 1.95   Mar 10 2006 09:25:22   rgreene
 * BD014762
 *
 *    Rev 1.94   Mar 03 2006 15:17:50   rgreene
 * BD014806  AG -collect Reason code - Cannot enter less than 3 digit code - all UDC validations coded same
 *
 *    Rev 1.93   Feb 21 2006 15:06:32   rgreene
 *  BE014743: Date format Support
 *
 *    Rev 1.92   Feb 16 2006 09:52:18   rgreene
 * BE013514 - License Plating - Allow RFS to gen L/P Number
 *
 *    Rev 1.91   Feb 13 2006 14:52:46   rgreene
 * BD014193: W/O Comp, IBSRCE = 0
 *
 *    Rev 1.90   Feb 13 2006 14:16:08   rgreene
 *  BD014221: Lot Exp Date - IBSRCE = 1
 *
 *    Rev 1.89   Feb 13 2006 10:58:50   rgreene
 * BE0134524 - L/P W/O Completions
 *
 *    Rev 1.88   Feb 07 2006 15:07:00   rgreene
 * BE014125: print wo labels also at beginning of workflow
 *
 *    Rev 1.87   Feb 06 2006 15:01:58   rgreene
 *  BD014193: W/O Comp, IBSRCE = 0
 *
 *    Rev 1.86   Feb 06 2006 14:57:40   rgreene
 * BD014195: W/O Completion, IBSRCE = 4
 *
 *    Rev 1.85   Feb 01 2006 14:45:44   rgreene
 * BD014232: W/O Completion - System generated S/N (IBSRCE = 5 and 6)
 *
 *    Rev 1.84   Feb 01 2006 13:50:12   rgreene
 * BD014213: QA/QTP - Label qty = 0 for IBSRCE = *BLANK
 *
 *    Rev 1.83   Jan 25 2006 14:43:32   rgreene
 * BE013524 - Work order Completions
 *
 *    Rev 1.82   Jan 22 2006 06:39:46   rgreene
 * BD014232: W/O Completion - System generated S/N (IBSRCE = 5 and 6)
 *
 *    Rev 1.81   Jan 13 2006 12:00:30   rgreene
 * BE013720 - Remove Psft Name
 *
 *    Rev 1.80   Jan 13 2006 11:56:40   rgreene
 * BE013720 - Remove Psft Name
 *
 *    Rev 1.79   Dec 14 2005 10:25:38   rgreene
 * BE013524 8.11 LP Work Order Completions
 *
 *    Rev 1.78   Nov 30 2005 16:18:22   rgreene
 * BD013719 Expired lot allowed in inv. adj. when workflow option should prevent it
 *
 *    Rev 1.77   Nov 01 2005 10:29:56   rgreene
 * BE013345 Workflow Logging
 *
 *    Rev 1.76   Oct 20 2005 09:44:06   rgreene
 * BE013131 Container Mgmt - Add Container in WO Completion
 *
 *    Rev 1.75   Sep 29 2005 10:21:02   rgreene
 * BD012885 Remove Lot Status Screens
 *
 *    Rev 1.74   Aug 26 2005 15:50:38   rgreene
 * BE012421: on-hand/available balance checking - along with clean up of F41021 derived class
 *
 *    Rev 1.73   Aug 24 2005 19:04:02   rgreene
 * BD011710: Incorrect qty validation on WO completion of serialized item
 *
 *    Rev 1.72   Aug 23 2005 16:57:50   rgreene
 * BD012489: 2745-Error: Lot/Serial Number Must be Entered
 *
 *    Rev 1.71   Aug 22 2005 17:25:16   PMonaco
 * BD012441 - Print check and messages display.
 *
 *    Rev 1.70   Aug 22 2005 12:57:00   rgreene
 * BD011987: Changes needed to serial processing in WO Completion
 *
 *    Rev 1.69   Aug 09 2005 15:06:24   rgreene
 *  BD012340: Printer name in enter printer screen.
 *
 *    Rev 1.68   Aug 06 2005 19:16:20   rgreene
 * BD012120: Enable Search button (F1) for CollectLot form
 *
 *    Rev 1.67   Aug 03 2005 15:56:46   rgreene
 * BD012318: Back button on collect printer - environment id
 *
 *    Rev 1.66   Aug 03 2005 11:18:10   rgreene
 * print label after send to ERP
 *
 *    Rev 1.65   Aug 02 2005 15:52:34   rgreene
 * BD012288: Barcode Print After Transaction Sent to ERP
 *
 *    Rev 1.64   Aug 01 2005 16:00:24   rgreene
 * BD012260: Default UOM for Scrap Qty
 *
 *    Rev 1.63   Aug 01 2005 15:57:22   rgreene
 * BE012262
 *
 *    Rev 1.62   Jul 27 2005 13:15:34   rgreene
 * Clean up the file open/close logic for new change to class. This omits the table object array that we were using
 *
 *    Rev 1.61   Jul 26 2005 10:04:10   rgreene
 * BD012166: WO Completion: Result list for Location search
 *
 *    Rev 1.60   Jul 14 2005 16:31:10   lsagan
 * BD011490: clear serial counter
 *
 *    Rev 1.59   Jul 08 2005 09:53:58   lsagan
 * BD012039: do not prompt for printer if label qty entered = 0
 *
 *    Rev 1.58   Jun 23 2005 21:47:54   rgreene
 * BD011871: WO Issue: Print Labels display with reg issue
 *
 *    Rev 1.57   Jun 22 2005 09:44:42   rgreene
 * BD011693: Prime Location Displayed in Distribution Workflows
 *
 *    Rev 1.56   Jun 19 2005 03:10:58   rgreene
 * BE011627: Tolerance Check in W/O Completion
 *
 *    Rev 1.55   Jun 16 2005 11:09:38   rgreene
 * BD011761: WO Completion: white screen when selecting from Status Search form
 *
 *    Rev 1.54   Jun 16 2005 10:59:04   rgreene
 * BD011758: WO Tracking: Scrap Qty not dspg on Scrap UOM form
 *
 *    Rev 1.53   Jun 15 2005 13:32:54   rgreene
 * BD011748: Barcode Label Printing from Within Workflow
 *
 *    Rev 1.52   Jun 14 2005 17:51:50   rgreene
 * BD011693: Prime Location Displayed in Distribution Workflows
 *
 *    Rev 1.51   Jun 13 2005 16:15:40   rgreene
 * BD011687: Inappropriate validation of branch/work order in work order issue
 *
 *    Rev 1.50   Jun 13 2005 15:58:00   rgreene
 * BD011680: F4 not working correctly on StoneridgeMasterPallet, page name CollectPrinter
 *
 *    Rev 1.49   Jun 13 2005 13:21:20   rgreene
 * BD011711: WO Completion did not enforce lot control rule
 *
 *    Rev 1.48   Jun 08 2005 17:21:26   rgreene
 * BD011648 BD011666 BD011484
 *
 *    Rev 1.47   Jun 07 2005 09:39:22   rgreene
 * Clean up comments
 *
 *    Rev 1.46   Jun 03 2005 07:14:26   rgreene
 * BE011627: Tolerance Check in W/O Completion
 *
 *    Rev 1.45   May 27 2005 16:21:14   rgreene
 * Remove "magic Number" dealing with lot process type and replace it with enumerator
 *
 *    Rev 1.44   May 23 2005 15:28:22   rgreene
 * 11534
 *
 *    Rev 1.43   May 20 2005 14:53:46   rgreene
 * BD011524: WO Complete - Error on 2nd pass through transaction
 *
 *    Rev 1.42   May 20 2005 14:45:06   rgreene
 *  BD011523: WO Completion -- Serialized item tries to write qty > 1 to a serial number
 *
 *    Rev 1.41   May 19 2005 16:27:22   rgreene
 * BD011487: Erroneous "Invalid key" error on CollectLotSerialExpDate in WO completion
 *
 *    Rev 1.40   May 18 2005 16:33:28   rgreene
 * QA issues
 *
 *    Rev 1.39   May 16 2005 22:03:12   rgreene
 * BD011303: Cycle Count Direct: System does not error on invalid value for qty - Rework quantity logic in jde workflows
 *
 *    Rev 1.38   May 13 2005 01:31:04   rgreene
 * Change to lot/serial number logic
 *
 *    Rev 1.37   May 11 2005 14:39:52   PMonaco
 * Chages to use ERP user name for RFUserID in transaction.  BE011300
 *
 *    Rev 1.36   May 11 2005 09:50:20   rgreene
 * BD011205
 *
 *    Rev 1.35   May 11 2005 06:40:20   rgreene
 * BD011247: WO Completion - number of labels
 *
 *    Rev 1.34   May 04 2005 15:22:36   rgreene
 * Mods per 5/3/05 QA session
 *
 *    Rev 1.33   Apr 25 2005 15:04:46   rgreene
 * Change List lookup on item to show O/H
 *
 *    Rev 1.32   Mar 29 2005 14:19:48   rgreene
 * qty check at barcode prompt
 *
 *    Rev 1.31   Mar 29 2005 13:15:54   rgreene
 * FSUG list
 *
 *    Rev 1.30   Mar 22 2005 08:58:38   rgreene
 * Problem with serial number collection and label printing
 *
 *    Rev 1.29   Mar 21 2005 08:23:28   rgreene
 * problem with null "last date entered" class variable
 *
 *    Rev 1.28   Mar 14 2005 18:06:42   rgreene
 * Problem with back event from lists
 *
 *    Rev 1.27   Mar 14 2005 15:15:08   rgreene
 * Problem with printer editing
 *
 *    Rev 1.26   Mar 11 2005 16:11:18   rgreene
 * Code Refactoring
 *
 *    Rev 1.25   Mar 09 2005 15:54:50   rgreene
 * Code Refactoring - round 1
 *
 *    Rev 1.24   Mar 07 2005 11:55:48   PMonaco
 * Refactor updates
 *
 *    Rev 1.23   Mar 03 2005 11:06:00   rgreene
 * QA testing - round 2 - BD010971
 *
 *    Rev 1.22   Feb 28 2005 11:38:30   PMonaco
 * Update to change erp calls
 *
 *    Rev 1.21   Feb 26 2005 18:59:40   rgreene
 * use Screen.Show for all lists in order to keep from placing them on the screen stack
 *
 *    Rev 1.20   Feb 23 2005 10:40:48   rgreene
 * correct form name issue with labels
 *
 *    Rev 1.19   Feb 23 2005 09:27:38   rgreene
 * Barcode label print files
 *
 *    Rev 1.18   Feb 21 2005 15:48:50   rgreene
 * code clean up
 *
 *    Rev 1.17   Feb 21 2005 15:34:08   PMonaco
 * Update to screen map
 *
 *    Rev 1.16   Feb 18 2005 21:38:40   rgreene
 * add copies to barcode labels
 *
 *    Rev 1.15   Feb 12 2005 20:40:12   rgreene
 * add printer search
 *
 *    Rev 1.14   Feb 11 2005 11:03:38   rgreene
 * barcode mods
 *
 *    Rev 1.13   Feb 10 2005 10:29:58   rgreene
 * Initial cut at Barcode labels
 *
 *    Rev 1.12   Jan 31 2005 13:41:20   rgreene
 * Correct issue with search screens not returning to proper spot
 *
 *    Rev 1.11   Jan 30 2005 02:20:56   rgreene
 * initial QA testing
 *
 *    Rev 1.10   Jan 29 2005 00:02:20   rgreene
 * change method of verifying location
 *
 *    Rev 1.9   Jan 18 2005 11:53:44   KarmaSherab
 * activate 'Back' on initial screen in workflow
 *
 *    Rev 1.8   Jan 14 2005 13:21:38   KarmaSherab
 * Clean up SQL
 *
 *    Rev 1.7   Jan 13 2005 14:13:30   KarmaSherab
 * clean up DISPOSE method
 *
 *    Rev 1.6   Jan 11 2005 17:44:34   KarmaSherab
 * add U/M list
 *
 *    Rev 1.5   Jan 10 2005 18:17:28   KarmaSherab
 * issues found during testing
 *
 *    Rev 1.4   Jan 04 2005 11:40:46   KarmaSherab
 * change naming to remove PeopleSoft, display messages after transaction sent to web service
 *
 *    Rev 1.3   Jan 03 2005 10:28:22   KarmaSherab
 * Empty location logic
 *
 *    Rev 1.2   Dec 21 2004 11:13:14   KarmaSherab
 * Pass quantities to JDE in primary U/M
 *
 *    Rev 1.1   Dec 17 2004 09:06:44   KarmaSherab
 * Document VT220 screens, verify WFO
 *
 *    Rev 1.0   Dec 03 2004 07:47:24   KarmaSherab
 * Initial Revision
 *
 *
*/

//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Collections;
using ICS.Database.TableObjects.JDE;
using ICS.Database.TableObjects.RFS;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;
using ICS.Display;
using ICS.ERPI.JDE;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.ShopFloor
{
    /// <summary>
    /// Screen Control Class For: StoneridgeMasterPallet Workflow
    /// </summary>
    public class StoneridgeMasterPallet : ICS.Workflow.Base.WorkflowDesign.Components.RFSWorkflowDesigner
    {
        #region Fields

        #region Tables

        // define tables used
        private F0005 _UserDefinedCodesTable; // JDE user defined codes
                                              //private F0006 _BranchPlantTable; // JDE Branch Master
        private F4100 _LocationMasterTable; // JDE location master
                                            //private F41001 _WarehouseControlTable; // Warehouse Constants Table
        private F41002 _tblUOMConversion; // jde UOM conversion tables
        private F41003 _tblUOMConversion2; // jde UOM conversion tables
                                           //private F4101 _ItemMasterTable; // JDE item master

        private F41021 _ItemLocationTable; // jde item/lot/location
        private F4108 _LotMasterTable; // jde lot master
        private F46011 _ItemUOMProfileTable;
        private F46L10 _LicensePlateMasterTable;
        private CustomJDETables.F4801 _WorkOrderMasterTable; // jde work order master
        private RFS_DataManipulator _DataManipulatorTable;
        private RFS_PrinterDefinition _DefinedPrintersTable; // RFS Defined Printers
        private RFS_PrintXref _PrinterCrossReferenceTable;
        private RFS_NextNumber _NextNumberTable;
        private DataSet _GeneratedLicensePlateNumbersDataSet;
        private DataTable _GeneratedLicensePlateNumbersTable;
        private F46L001 _LicensePlateNumberingConstantsTable;
        private CustomJDETables.F4102 _ItemBranchTable; // JDE item/Branch
        private CustomJDETables.F4211 _SalesOrderDetailTable;
        private CustomJDETables.F3112 _WorkOrderRoutingTable;
        private CustomJDETables.F3112Z1 _WorkOrderTimeTableZfile;
        private CustomJDETables.F4801Z1 _WorkOrderMasterTableZfile;
        private CustomJDETables.F5846001 _SerialTable;
        private CustomJDETables.F3109 _LineItemMasterTable;
        private CustomJDETables.F5642001 _JobNumberTableZfile;
        private CustomJDETables.F49211 _SalesOrderDetailTagTable;
        private CustomJDETables.F59RFCTL _PalletSerialTable;

        #endregion

        #region Screens

        ICS.Workflow.Base.JDE.Components.BranchPlantScreen _CollectBranch;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabels;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabelsStartOfOrder;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinter;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinterStartOfOrder;
        // list/lookup screens
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _BranchPlantList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _DefinedPrintersTableList;

        #endregion

        #region Collect/Work data

        private string _PrinterName;
        private string _BranchPlant; // work order Branch
        private string _PrimaryItemNumber; // primary item number
        private string _Location; // issue location
        private string _LotNumber; // lot/serial number
        private string _LotSerialExpDate; // lot/serial expiration date
        private string _ToLotGrade;
        private string _ToLotPotency;
        private double _TransactionQuantity; // Used to hold the scrapped quantity and UOM converted
        private string _UnitOfMeasure;       // to the primary
        private bool _StartOfOrder;
        private double _ScrapQuantity;      // Used to hold the scrapped quantity and UOM converted
        private string _ScrapUnitOfMeasure; // to the primary

        private double _ShortItemNumber; // short item number
        private string _LongItemNumber; // second item number
        private string _ThirdItemNumber; // third item number
        private string _LotStatus; // lot status


        private string _PrinterAddress;

        private string _IsLocationControlActive; // Y/N flag to denote if whse is using location control
        private int _SerialNumbersScanned; // s/n scanned
        private string _LicensePlate;
        private string _LicensePlateProcessingType = "";

        private bool _FromSerialNumberEntry = false;

        private string _LocationSeparator;
        private string[] _AllowedLotHoldCodes = null; // allowed lot hold codes
        private string[] _AllowedWorkOrderStatusCodes = null;

        private bool _FromGenLP = false;

        private bool _LPLicense = false;
        private string[] _CaseUOM;

        private double _PalletNumber;
        private string _SerialNumber;
        #endregion
        protected ICS.Workflow.Base.JDE.Components.ItemScreen _CollectItemNumber;
        private string[] _LPEDER;
        protected GenericScreen CollectPalletNumber;
        protected GenericScreen CollectCartonSerial;
        protected GenericScreen RemoveCartonSerial;
        protected ICS.Workflow.Base.JDE.Components.YesNoResponseScreen ConfirmSerialRemove;


        #region ERPI

        private ICS.ERPI.JDE.WOCompletion _WOCompletionTransaction = new WOCompletion();

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Workflow : StoneridgeMasterPallet
        /// </summary>
        /// <param name="userSession">Contains UserSession info for controling screen display and DAL.</param>
        public StoneridgeMasterPallet(UserSession userSession) : base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        } // end StoneridgeMasterPallet constructor

        #endregion

        #region Dispose

        /// <summary>
        /// Used to perform any actions necessary during the destruction of an object of
        /// this class like cleanup, etc.
        /// </summary>
        public override void Dispose()
        {
            _GeneratedLicensePlateNumbersDataSet.Dispose();
            ScreenMap.ClearAllWorkflows();
            Transactions.ClearTransactions();
            ScreenMap.Clear("Error");
            base.Dispose();
        } // end Dispose

        #endregion

        #region Methods

        #region Initialize

        /// <summary>
        /// Initializes Workflow and Sets screens and Table Objects.
        /// Declares Next with starting screen
        /// </summary>
        protected override void Initialize()
        {
            #region Table Inits

            _UserDefinedCodesTable = new F0005(UserSession.DataAccess, "");
            //_BranchPlantTable = new F0006(UserSession.DataAccess, "");
            _LocationMasterTable = new F4100(UserSession.DataAccess, "");
            //_WarehouseControlTable = new F41001(UserSession.DataAccess, "");
            _tblUOMConversion = new F41002(UserSession.DataAccess, "");
            //_ItemMasterTable = new F4101(UserSession.DataAccess, "");

            _ItemLocationTable = new F41021(UserSession.DataAccess, "");
            _LotMasterTable = new F4108(UserSession.DataAccess, "");
            _ItemUOMProfileTable = new F46011(UserSession.DataAccess, "");
            _WorkOrderMasterTable = new CustomJDETables.F4801(UserSession.DataAccess, "");
            _DataManipulatorTable = new RFS_DataManipulator(UserSession.DataAccess, "");
            _DefinedPrintersTable = new RFS_PrinterDefinition(UserSession.DataAccess, "");
            _PrinterCrossReferenceTable = new RFS_PrintXref(UserSession.DataAccess, "");
            _NextNumberTable = new RFS_NextNumber(UserSession.DataAccess, "");
            _LicensePlateNumberingConstantsTable = new F46L001(UserSession.DataAccess, "");
            _tblUOMConversion2 = new F41003(UserSession.DataAccess, "");

            _ItemBranchTable = new CustomJDETables.F4102(UserSession.DataAccess, "");
            _SalesOrderDetailTable = new CustomJDETables.F4211(UserSession.DataAccess, "");
            _WorkOrderRoutingTable = new CustomJDETables.F3112(UserSession.DataAccess, "");
            _WorkOrderTimeTableZfile = new CustomJDETables.F3112Z1(UserSession.DataAccess, "");
            _SerialTable = new CustomJDETables.F5846001(UserSession.DataAccess, "");
            _LineItemMasterTable = new CustomJDETables.F3109(UserSession.DataAccess, "");
            _JobNumberTableZfile = new CustomJDETables.F5642001(UserSession.DataAccess, "");
            _SalesOrderDetailTagTable = new CustomJDETables.F49211(UserSession.DataAccess, "");
            _PalletSerialTable = new CustomJDETables.F59RFCTL(UserSession.DataAccess, "");
            #endregion

            ClearTransaction();
            // Setting up workflow options
            char[] Delimiter = { ' ' };
            string sTemp = WFOptions.RetrieveOptionValue("AllowedWorkOrderStatus");
            _AllowedWorkOrderStatusCodes = sTemp.Split(Delimiter);
            sTemp = WFOptions.RetrieveOptionValue("AllowedLotHoldCodes");
            _AllowedLotHoldCodes = sTemp.Split(Delimiter);
            sTemp = WFOptions.RetrieveOptionValue("CaseUOM");
            _CaseUOM = sTemp.Split(Delimiter);
            _LPEDER = new string[]{"0","1","2","3","4","5","6","7","8","9","A","B","C","D","E","F","G","H","I","J","K","L","M",
                                    "N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
            // License plate tables
            if (UserSession.IsModuleLicensed("JLP"))
            {
                _LPLicense = true;
                _LicensePlateMasterTable = new F46L10(UserSession.DataAccess, "");
            } // end if
            CreateDataSet();
        } // end Initialize

        #endregion

        #region CollectBranch

        //*****************************************************************
        // CollectBranch
        //*****************************************************************

        #region CollectBranch_EnterEvent

        /// <summary>
        /// Enter event for the Collect Branch Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectBranch_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _BranchPlant = e.UserInput.ToString().ToUpper();
            BranchPlantEnter();
        }

        private void BranchPlantEnter()
        {
            _CollectBranch.BranchPlantsTable.ResetAll();
            _CollectBranch.BranchPlantsTable.BranchPlant = _BranchPlant;

            // Validate the Branch entered against the F0006 table
            if (_CollectBranch.BranchPlantsTable.ValidBranchPlant())
            {
                _CollectBranch.Validate(_BranchPlant);
                _BranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _CollectBranch.BranchPlantsTable.MCMCU);
                _CollectBranch.WarehouseConstantsTable.ResetAll();
                _CollectBranch.WarehouseConstantsTable.CIMCU = _BranchPlant;
                _CollectBranch.WarehouseConstantsTable.Fetch();
                if (_CollectBranch.WarehouseConstantsTable.Rows.Count > 0)
                {
                    _LocationSeparator = _CollectBranch.WarehouseConstantsTable.CISEPL;
                    _IsLocationControlActive = _CollectBranch.WarehouseConstantsTable.CILCTL;
                } // end if
                else
                {
                    _IsLocationControlActive = "N";
                } // end else
                _LicensePlateNumberingConstantsTable.ResetAll();
                _LicensePlateNumberingConstantsTable.BranchPlant = _BranchPlant;

            } // end if
            else
            {
                Error("MCU", _BranchPlant);
            } // end else
        } // end CollectBranch_EnterEvent

        #endregion

        #region BranchPlantList

        //*****************************************************************
        // BranchPlantList
        //*****************************************************************

        #region BranchPlantList_EnterEvent

        /// <summary>
        /// Enter event for the BranchPlantList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BranchPlantList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectBranch.BranchPlantsTable.UpdateCurrentRow();
            _BranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _CollectBranch.BranchPlantsTable.MCMCU);
            BranchPlantEnter();
        } // end BranchPlantList_EnterEvent

        #endregion

        #endregion

        #endregion


        #region LoadScreenMap

        /// <summary>
        /// LoadScreenMap is used to initialize data and set ScreenMaps such as the Primary
        /// Item Number and Location, WorkOrderNumber, etc.
        /// </summary>
        private void LoadScreenMap()
        {
            //Set item screen info even though we aren't showing it
            // b/c other screens have reference to it.
            _CollectItemNumber.Validate(_WorkOrderMasterTable.WALITM);

            _ShortItemNumber = Convert.ToDouble(_WorkOrderMasterTable.WAITM);
            _ThirdItemNumber = _WorkOrderMasterTable.WAAITM;
            _LongItemNumber = _WorkOrderMasterTable.WALITM;

            //ScreenMap.Add("WorkOrderNumber", _WorkOrderNumber.ToString());
            ScreenMap.Add("ItemNumber", _LongItemNumber);

            _CollectItemNumber.ItemsTable.ResetAll();
            _CollectItemNumber.ItemsTable.IMITM = _ShortItemNumber;
            _CollectItemNumber.ItemsTable.Fetch();
            switch (_CollectBranch.WarehouseConstantsTable._DesignatedPrimaryItemNumber)
            {
                case PrimaryItemNumberDesignations.ShortNumber:
                    _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMITM.ToString();
                    break;
                case PrimaryItemNumberDesignations.LongNumber:
                    _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMLITM;
                    break;
                case PrimaryItemNumberDesignations.ThirdNumber:
                    _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMAITM;
                    break;
            } // end switch

            _LocationMasterTable.ResetAll();
            _LocationMasterTable.inLOCN = _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant, _ShortItemNumber);
            _LocationMasterTable.MCU = _BranchPlant;
            if (_LocationMasterTable.ValidateLocation())
            {
                ScreenMap.Add("PrimaryLocation", _LocationMasterTable.sscnlocn);
            } // end if

            _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
            _LotNumber = _WorkOrderMasterTable.WALOTN;
        } // end LoadScreenMap

        #endregion

        #region ClearTransaction

        /// <summary>
        /// ClearTransactions clears the data fields to start a new transaction.
        /// </summary>
        private void ClearTransaction()
        {
            _ShortItemNumber = 0; // short item
            _PrimaryItemNumber = ""; // primary item
            _LongItemNumber = ""; // second item number
            _ThirdItemNumber = ""; // third item number
            _LotStatus = ""; // lot status
            _Location = ""; // location
            _LotNumber = ""; // lot number
            _LotSerialExpDate = ""; // lot/serial expiration date
            _TransactionQuantity = 0; // quantity Completed
            _UnitOfMeasure = ""; // completed unit of measure
            _ScrapQuantity = 0; // quantity scrapped
            _ScrapUnitOfMeasure = ""; // scrap uom
            _SerialNumbersScanned = 0;
            _LicensePlateProcessingType = "";
            _LicensePlate = "";
            _FromSerialNumberEntry = false;
            _ToLotGrade = "";
            _ToLotPotency = "";
            ScreenMap.Add("LotNumber", _LotNumber);

        } // end ClearTransaction

        #endregion ClearTransaction

        #region Label Printing

        //*****************************************************************
        // Label Printing
        //*****************************************************************

        #region CollectPrinter

        //*****************************************************************
        // CollectPrinter
        //*****************************************************************

        #region CollectPrinter_EnterEvent

        /// <summary>
        /// Enter event for the CollectPrinter Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinter_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // Validating that the printer exists for the environment in which they are logged.
            _PrinterName = e.UserInput.ToString().Trim();
            if (this.PrintSystem.ValidPrinter(_PrinterName))
            {
                _PrinterAddress = PrintSystem.PrinterAddress;

                if (_FromGenLP)
                {
                    // Print a label for each License Plate created
                    for (int i = 0; i < _GeneratedLicensePlateNumbersTable.Rows.Count; i++)
                    {
                        _LicensePlate = _GeneratedLicensePlateNumbersTable.Rows[i]["Number"].ToString().Trim();
                        PrintBarCode();
                    } // end for
                } // end if
                else
                {
                    PrintBarCode();
                } // end else

                // Determining which screen to go to.  If the user is collect serials, go back
                // to finish collecting serials until all serials have been scanned; otherwise,
                // start the workflow over again by collecting a new work order.
                if (_FromSerialNumberEntry && (_SerialNumbersScanned < _TransactionQuantity))
                {
                    //Back(_CollectSerialNumber);
                } // end if
                else
                {
                    ClearTransaction();
                    //Back(_CollectWorkOrderNumber);
                } // end else
            } // end if
            else
            {
                Error(this.PrintSystem.ErrorMessageVocabulary);
            } // end else
        } // end CollectPrinter_EnterEvent

        #endregion

        #region CollectPrinter_ListEvent

        /// <summary>
        /// List event for the CollectPrinter Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinter_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            //_StartOfOrder = false;
            _DefinedPrintersTable.FetchPrintersByUserWorkflow(UserSession.UserId, WorkflowID);
            _DefinedPrintersTableList.Show();
        } // end CollectPrinter_ListEvent

        #endregion

        #endregion

        #region DefinedPrinters

        //*****************************************************************
        // DefinedPrinters
        //*****************************************************************

        #region DefinedPrinters_EnterEvent

        /// <summary>
        /// Enter event for the DefinedPrinters Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefinedPrinters_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DefinedPrintersTable.UpdateCurrentRow();
            _PrinterName = _DefinedPrintersTable.PrinterName;
            if (this.PrintSystem.ValidPrinter(_PrinterName))
            {
                _PrinterAddress = PrintSystem.PrinterAddress;
                //PrintBarCode();
                Next(_PrintBarCodeLabelsStartOfOrder);
                /*if (_StartOfOrder)
                {
                    Next(_CollectQuantity);
                } // end if
                else
                {
                    if ((_FromSerialNumberEntry) && (_SerialNumbersScanned < _TransactionQuantity))
                    {
                        Back(_CollectSerialNumber);
                    } // end if
                    else
                    {
                        ClearTransaction();
                        Back(_CollectWorkOrderNumber);
                    } // end else
                } // end else*/
            }
            else
                Error(this.PrintSystem.ErrorMessageVocabulary);
        } // end DefinedPrinters_EnterEvent

        #endregion

        #endregion

        #region StartOfOrder Label Printing

        #region PrintBarCodeLabelsStartOfOrder_EnterEvent

        /// <summary>
        /// Enter event for the PrintBarCodeLabelsStartOfOrder Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintBarCodeLabelsStartOfOrder_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if ((e.UserInput.ToString().Trim().Length > 0) && (e.UserInput.ToString().Trim() != "0"))
            {
                try
                {
                    PrintSystem.Copies = e.UserInput.ToInt32();

                    // Ensuring that a negative label quantity is not entered
                    if (PrintSystem.Copies < 0)
                    {
                        throw (new ArgumentException("Negative label qty entered"));
                    } // end if

                    // Checking to make sure that the max number of labels is not exceeded
                    else if (PrintSystem.Copies > WFOptions.GetInt32WFOptionValue("MaxLabels"))
                    {
                        Error("MAXEXCEEDED", e.UserInput.ToString());
                    } // end else if
                    else
                    {
                        Log.Debug($"<***> DefaultPrinter {PrintSystem.DefaultPrinter}");
                        // Trying to locate the Label Format to get the default printer
                        PrintSystem.Reset();
                        PrintSystem.Branch = _BranchPlant;
                        PrintSystem.ItemNumber = _PrimaryItemNumber;
                        PrintSystem.Location = _LocationMasterTable.stbllocn;
                        if (PrintSystem.DefaultPrinter.Trim().Length == 0)
                        {
                            Error("LabelFormat");
                        } // end if
                        else
                        {
                            Next(CollectPalletNumber);
                        } // end else
                    } // end else
                } // end try
                catch (RFS_Exception ex)
                {
                    Error(ex.ErrorName);
                } // end catch
                catch
                {
                    Error("QUANTITY", e.UserInput.ToString());
                } // end catch
            } // end if
            else
            {
                //Next(_CollectQuantity);
            } // end else
        } // end PrintBarCodeLabelsStartOfOrder_EnterEvent

        #endregion

        #region CollectPrinterStartOfOrder_EnterEvent

        /// <summary>
        /// Enter event for the CollectPrinterStartOfOrder Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinterStartOfOrder_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PrinterName = e.UserInput.ToString().Trim();
            if (PrintSystem.ValidPrinter(_PrinterName))
            {
                _PrinterAddress = _DefinedPrintersTable.Address;
            }
            /*if (_DefinedPrintersTable.ValidateUserPrinter(UserSession.UserId, _PrinterName))
            {
                _PrinterAddress = _DefinedPrintersTable.Address;
                Log.Debug($"<***> SoO PrinterAddress {_PrinterAddress}");
                //PrintBarCode();
            }*/ // end if
            else
            {
                Error("INVALIDPRINTER");
            } // end else
        } // end CollectPrinterStartOfOrder_EnterEvent

        #endregion

        #region CollectPrinterStartOfOrder_ListEvent

        /// <summary>
        /// List event for the CollectPrinterStartOfOrder Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinterStartOfOrder_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _StartOfOrder = true;
            _DefinedPrintersTable.FetchPrintersByUserWorkflow(UserSession.UserId, WorkflowID);
        } // end CollectPrinterStartOfOrder_ListEvent

        #endregion

        #endregion

        #region PrintBarCodeLabels

        //*****************************************************************
        // PrintBarCodeLabels
        //*****************************************************************

        #region PrintBarCodeLabels_EnterEvent

        /// <summary>
        /// Enter event for the PrintBarCodeLabels Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintBarCodeLabels_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToString().Trim().Length > 0 && e.UserInput.ToString().Trim() != "0")
            {
                try
                {
                    PrintSystem.Copies = e.UserInput.ToInt32();
                    if (PrintSystem.Copies < 0)
                    {
                        throw (new ArgumentException("Negative label qty entered"));
                    } // end if
                    else if (PrintSystem.Copies > WFOptions.GetInt32WFOptionValue("MaxLabels"))
                    {
                        Error("MAXEXCEEDED", e.UserInput.ToString());
                    } // end else if
                    else
                    {
                        PrintSystem.Reset();
                        PrintSystem.Branch = _BranchPlant;
                        PrintSystem.ItemNumber = _PrimaryItemNumber;
                        PrintSystem.Location = _LocationMasterTable.stbllocn;
                        if (PrintSystem.DefaultPrinter.Trim().Length == 0)
                        {
                            Error("LabelFormat");
                        } // end if
                        else
                        {
                            Next(_CollectPrinter, PrintSystem.DefaultPrinter);
                        } // end else
                    } // end else
                } // end try
                catch (RFS_Exception ex)
                {
                    Error(ex.ErrorName);
                } // end catch
                catch
                {
                    Error("QUANTITY", e.UserInput.ToString());
                } // end catch
            } // end if
            else if ((_FromSerialNumberEntry) && (_SerialNumbersScanned < _TransactionQuantity))
            {
                //Back(_CollectSerialNumber);
            } // end else if
            else
            {
                ClearTransaction();
                //Back(_CollectWorkOrderNumber);
            } // end else
        } // end PrintBarCodeLabels_EnterEvent

        #endregion

        #endregion

        #region PrintBarCode

        /// <summary>
        /// PrintBarCode initializes the Printing object with the Printer name, address, etc.
        /// and sets the field data for the dropfile and prints the dropfile.
        /// </summary>
        private void PrintBarCode()
        {
            DateTime PackDate = new DateTime();
            if(_BranchPlant.Trim().ToUpper() == "73W01")
            {
                PackDate = DateTime.Now.AddHours(-2);
            }
            else
            {
                PackDate = DateTime.Now;
            }
            PrintSystem.Reset();
            PrintSystem.DropFilePathOverride = WFOptions.RetrieveOptionValue("DropFilePath");
            PrintSystem.PrinterAddress = _PrinterAddress;
            PrintSystem.PrinterName = _PrinterName;
            PrintSystem.ContainerType = "";
            if ((_LPLicense)
                && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LicensePlate", _LicensePlate, false);
            } // end if
              //reset all of the job number label data
            for (int i = 1; i < 33; i++)
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_JobNumber{i}", "", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartNumber{i}", "", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartQty{i}", "", false);
            }
            /*PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_DocumentNumber", _WorkOrderMasterTable.WADOCO.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_DocumentType", _WorkOrderMasterTable.WADCTO, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ThirdItemNumber", _CollectItemNumber.ItemsTable.IMAITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_SecondItemNumber", _CollectItemNumber.ItemsTable.IMLITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ShortItemNumber", _CollectItemNumber.ItemsTable.IMITM.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Description1", _CollectItemNumber.ItemsTable.IMDSC1, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Branch", _CollectBranch.BranchPlantsTable.MCMCU, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", _LotNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_TransactionQuantity", _TransactionQuantity.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_UnitOfMeasure", _UnitOfMeasure, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LocationMasterTable", _LocationMasterTable.sscnlocn, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_PrimaryItemNumber", _PrimaryItemNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", _LotNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotSerialExpDate", _LotSerialExpDate, false);
            
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SRIPartNumber", JNComp.PartNumber, false);// _CollectItemNumber.ItemsTable.IMLITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartDesc", JNComp.PartDescription, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _TransactionQuantity.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", _NextSerialNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_JobNumber", JNComp.JobNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_WorkOrder", JNComp.WorkOrderNumber, false);
            _SerialTable.GetSerialNumber(_BranchPlant, JNComp.SerialNumber, false);
            */
            if(_SerialTable.LPURCD == "0")
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_CartonType", "R", false);
            }
            else
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_CartonType", "E", false);
            }
            
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateDay", PackDate.Day.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateMonth", PackDate.Month.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateYear", PackDate.Year.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.TimeOfDay.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);


            PrintSystem.ExtraPrintData.NewRow();
            if (!PrintSystem.Print())
            {
                DisplayMessage(PrintSystem.ErrorMessageVocabulary);
            } // end if
        } // end PrintBarCode

        #endregion
        #region PrintMasterCartonLabel
        private void PrintMasterCartonLabel()
        {
            DateTime PackDate = new DateTime();
            if (_BranchPlant.Trim().ToUpper() == "73W01")
            {
                PackDate = DateTime.Now.AddHours(-2);
            }
            else
            {
                PackDate = DateTime.Now;
            }
            PrintSystem.Reset();
            PrintSystem.DropFilePathOverride = WFOptions.RetrieveOptionValue("DropFilePath");
            PrintSystem.PrinterAddress = _PrinterAddress;
            PrintSystem.PrinterName = _PrinterName;
            PrintSystem.ContainerType = "M";
            if ((_LPLicense)
                && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LicensePlate", _LicensePlate, false);
            } // end if
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", _PalletNumber.ToString(), false);
            int serialcount = 1;
            //reset all of the job number label data
            for(int i = 1; i < 129; i++)
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialNumber{i}", "", false);
            }
            _SerialTable.GetSerialNumbers(_BranchPlant, _PalletNumber.ToString());
            foreach(DataRow dr in _SerialTable.Rows)
            {
                _SerialTable.CurrentRow = dr;

                Log.Debug($"<***>SerialNumber = {_SerialTable.LPFRVNUM}");
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialNumber{serialcount}", _SerialTable.LPLPNU, false);
                serialcount++;
            }
            
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateDay", PackDate.Day.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateMonth", PackDate.Month.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateYear", PackDate.Year.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.ToString("HH:mm"), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);

            PrintSystem.ExtraPrintData.NewRow();
            if (!PrintSystem.Print())
            {
                DisplayMessage(PrintSystem.ErrorMessageVocabulary);
            } // end if
        } // end PrintMasterCartonLabel
        #endregion
        #endregion

        #region SendToERP

        private void setToLocation()
        {
            _WOCompletionTransaction.Location = _Location.Trim();
            if (_LocationMasterTable.ValidateLocation(_CollectBranch.BranchPlantsTable.MCMCU.Trim(), _Location))
            {
                _WOCompletionTransaction.ToLocnPutZone = _LocationMasterTable.LMPZON.Trim();
                _WOCompletionTransaction.ToLocnPickZone = _LocationMasterTable.LMKZON.Trim();
                _WOCompletionTransaction.ToLocnReplZone = _LocationMasterTable.LMZONR.Trim();
                _WOCompletionTransaction.ToLocnDimGroup = _LocationMasterTable.LMSTY1.Trim();
            }
        }
        private void setItem()
        {
            _WOCompletionTransaction.PrimaryItemNumber = _PrimaryItemNumber;
            _WOCompletionTransaction.ShortItemNumber = _ShortItemNumber.ToString();
            _WOCompletionTransaction.SecondItemNumber = _LongItemNumber;
            _WOCompletionTransaction.ThirdItemNumber = _ThirdItemNumber;
            _WOCompletionTransaction.ItemDescription1 = _CollectItemNumber.ItemsTable.IMDSC1; // Needed for WOW
            _WOCompletionTransaction.ItemDescription2 = _CollectItemNumber.ItemsTable.IMDSC1; // Needed for WOW
            _WOCompletionTransaction.ItemDimGroup = _CollectItemNumber.ItemBranchesTable.IBPRP6.Trim();
            _WOCompletionTransaction.ItemLotProcType = _CollectItemNumber.ItemBranchesTable.IBSRCE;
        }
        /// <summary>
        /// SendToErp initializes the ERP transaction fields and sends the transaction
        /// to the ERP system plugin.
        /// </summary>


        #endregion SendToERP

        #region CreateDataSet

        /// <summary>
        /// CreateDataSet creates a data set for the GeneratedLicensePlateNumbers with
        /// a table for the GeneratedLicensePlateNumbers that has one field (Number).
        /// </summary>
        private void CreateDataSet()
        {
            _GeneratedLicensePlateNumbersDataSet = new DataSet("GeneratedLicensePlateNumbersDataSet");
            _GeneratedLicensePlateNumbersTable = new DataTable("GeneratedLicensePlateNumbersTable");
            _GeneratedLicensePlateNumbersTable.Columns.Add("Number");
            _GeneratedLicensePlateNumbersDataSet.Tables.Add(_GeneratedLicensePlateNumbersTable);
        }

        public StoneridgeMasterPallet()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._PrintBarCodeLabelsStartOfOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectBranch = new ICS.Workflow.Base.JDE.Components.BranchPlantScreen(this);
            this._BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectPrinterStartOfOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._DefinedPrintersTableList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectItemNumber = new ICS.Workflow.Base.JDE.Components.ItemScreen(this);
            this._CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._PrintBarCodeLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectPalletNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectCartonSerial = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.RemoveCartonSerial = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ConfirmSerialRemove = new ICS.Workflow.Base.JDE.Components.YesNoResponseScreen(this);
            // 
            // _PrintBarCodeLabelsStartOfOrder
            // 
            this._PrintBarCodeLabelsStartOfOrder.ScreenName = "PrintBarCodeLabelsStartOfOrder";
            this._PrintBarCodeLabelsStartOfOrder.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabelsStartOfOrder_EnterEvent);
            this._PrintBarCodeLabelsStartOfOrder.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._PrintBarCodeLabelsStartOfOrder_LoadEvent);
            this._PrintBarCodeLabelsStartOfOrder.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this._PrintBarCodeLabelsStartOfOrder_F2Method);
            // 
            // _CollectBranch
            // 
            this._CollectBranch.ListScreen = this._BranchPlantList;
            this._CollectBranch.ListScreenControl = this._BranchPlantList;
            this._CollectBranch.NextScreen = this._CollectPrinterStartOfOrder;
            this._CollectBranch.ScreenName = "CollectBranch";
            this._CollectBranch.WarehouseConstantsTable = null;
            this._CollectBranch.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranch_EnterEvent);
            this._CollectBranch.BranchPlantsList.BackScreen = this._CollectBranch;
            this._CollectBranch.BranchPlantsList.ScreenName = "F0006_List";
            // 
            // _BranchPlantList
            // 
            this._BranchPlantList.ListScreenParent = this._CollectBranch;
            this._BranchPlantList.ParentScreen = this._CollectBranch;
            this._BranchPlantList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BranchPlantList_EnterEvent);
            this._BranchPlantList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._BranchPlantList_LoadEvent);
            // 
            // _CollectPrinterStartOfOrder
            // 
            this._CollectPrinterStartOfOrder.ListScreen = this._DefinedPrintersTableList;
            this._CollectPrinterStartOfOrder.ListScreenControl = this._DefinedPrintersTableList;
            this._CollectPrinterStartOfOrder.NextScreen = this._PrintBarCodeLabelsStartOfOrder;
            this._CollectPrinterStartOfOrder.ScreenName = "CollectPrinterStartOfOrder";
            this._CollectPrinterStartOfOrder.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinterStartOfOrder_EnterEvent);
            this._CollectPrinterStartOfOrder.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinterStartOfOrder_ListEvent);
            this._CollectPrinterStartOfOrder.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this._CollectPrinterStartOfOrder_F2Method);
            // 
            // _DefinedPrintersTableList
            // 
            this._DefinedPrintersTableList.ListScreenParent = this._CollectPrinterStartOfOrder;
            this._DefinedPrintersTableList.ParentScreen = this._CollectPrinterStartOfOrder;
            this._DefinedPrintersTableList.ScreenName = "FRFS_Printers";
            this._DefinedPrintersTableList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.DefinedPrinters_EnterEvent);
            this._DefinedPrintersTableList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._DefinedPrintersTableList_LoadEvent);
            // 
            // _CollectItemNumber
            // 
            this._CollectItemNumber.BranchScreen = this._CollectBranch;
            this._CollectItemNumber.ItemBranchesTable = null;
            this._CollectItemNumber.ItemsTable = null;
            this._CollectItemNumber.ScreenName = "CollectItemNumber";
            // 
            // _CollectPrinter
            // 
            this._CollectPrinter.ScreenName = "CollectPrinter";
            this._CollectPrinter.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_EnterEvent);
            this._CollectPrinter.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_ListEvent);
            // 
            // _PrintBarCodeLabels
            // 
            this._PrintBarCodeLabels.ScreenName = "PrintBarCodeLabels";
            this._PrintBarCodeLabels.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabels_EnterEvent);
            // 
            // CollectPalletNumber
            // 
            this.CollectPalletNumber.ScreenName = "CollectPalletNumber";
            this.CollectPalletNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectPalletNumber_ValidateMethod);
            this.CollectPalletNumber.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectPalletNumber_F2Method);
            // 
            // CollectCartonSerial
            // 
            this.CollectCartonSerial.ScreenName = "CollectCartonSerial";
            this.CollectCartonSerial.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectCartonSerial_EnterValidationEvent);
            this.CollectCartonSerial.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectCartonSerial_F2Method);
            this.CollectCartonSerial.F6Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectCartonSerial_F6Method);
            // 
            // RemoveCartonSerial
            // 
            this.RemoveCartonSerial.ScreenName = "RemoveCartonSerial";
            this.RemoveCartonSerial.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.RemoveCartonSerial_EnterValidationEvent);
            // 
            // ConfirmSerialRemove
            // 
            this.ConfirmSerialRemove.ScreenName = "ConfirmSerialRemove";
            this.ConfirmSerialRemove.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmSerialRemove_EnterValidationEvent);
            // 
            // StoneridgeMasterPallet
            // 
            this.StartScreen = this._CollectBranch;
            this.WorkflowID = "StoneridgeMasterPallet";

        }

        private void _LicensePlateMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {

        }

        private void _ItemBalanceList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {

        }

        private void _EmptyLocationsList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _EmptyLocationsList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _DefinedPrintersTableList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {

        }

        private void _LocationMasterList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {

        }

        private void _LocationMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _UnitOfMeasureConversionList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }


        private void _UserDefinedCodesList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _BranchPlantList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectBranch.BranchPlantsTable.FetchBranchPlants();
        }

        private void _WorkOrderMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkOrderMasterTable.ShowWorkOrders(_AllowedWorkOrderStatusCodes, _BranchPlant, WFOptions.RetrieveOptionValue("WODocType"));
        }

        private void _CollectSerialNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            DefaultRFSGeneratedLotNumber();
        }
        private void DefaultRFSGeneratedLotNumber()
        {
            if (WFOptions.GetYesNoWFOptionValue("RFSGenLotSerialNumber"))
            {
                int NextNumber = 0;
                string FormattedNextNumber = "";
                if (_NextNumberTable.GetNextNumber("LotNumber", out NextNumber, out FormattedNextNumber))
                    ScreenHandler.DefaultText = FormattedNextNumber;
            }
        }

        private void _CollectLot_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            DefaultRFSGeneratedLotNumber();
        }

        private void _CollectLotSerialExpDate_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("DateFormat", WFOptions.RetrieveOptionValue("DateFormat").ToString());


        }

        // end CreateDataSet

        #endregion


        #endregion


        private void _PrintBarCodeLabelsStartOfOrder_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WFOptions.RetrieveOptionValue("DeafaultCartonLabelPerCarton");
        }


        private bool CollectPalletNumber_ValidateMethod(string PalletNumber)
        {
            if(double.TryParse(PalletNumber, out _PalletNumber))
            {
                if (_PalletSerialTable.GetMasterCarton(_BranchPlant, PalletNumber))
                {
                    ScreenMap.Add("Serial", "");
                    ScreenMap.Add("PalletNumber", PalletNumber);
                    Next(CollectCartonSerial);
                }
                else
                {
                    throw new RFS_Exception("SERIALNOTAVAILBLE");
                }
            }
            else
            {
                throw new RFS_Exception("Not Numeric");
            }
            return true;
        }

        private void CollectCartonSerial_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _SerialNumber = e.UserInput.ToString();
            if(_SerialTable.GetSerialNumber(_BranchPlant,_SerialNumber,false))
            {
                if(_SerialTable.GetShipment(_BranchPlant, _SerialNumber))
                {
                    if(_SerialTable.GetSerialNumber(_BranchPlant, _SerialNumber, false))
                    {
                        if(_SerialTable.LPPLPNU.Trim() == _PalletNumber.ToString())
                        {
                            ScreenMap.Add("Serial", "");
                            throw new RFS_Exception("ALREADYSCANNED");
                        }
                        if(_SerialTable.LPPLPNU.Trim() != "")
                        {
                            ScreenMap.Add("Serial", "");
                            throw new RFS_Exception("ONMASTERPALLET");
                            
                        }
                        if(_SerialTable.AddtoMasterPallet(_BranchPlant, _SerialNumber, _PalletNumber.ToString()))
                        {
                            ScreenMap.Add("Serial", _SerialNumber + " Added");
                            Next(CollectCartonSerial);
                        }
                        else
                        {
                            ScreenMap.Add("Serial", "");
                            throw new RFS_Exception("SERIALNOTADDED");
                        }
                    }
                }
                else
                {
                    ScreenMap.Add("Serial", "");
                    throw new RFS_Exception("CTNONSHIPMENT");
                }
            }
            else
            {
                ScreenMap.Add("Serial", "");
                throw new RFS_Exception("NOCTNFOUND");
            }

        }
        #region F2 Methods
        private bool CollectCartonSerial_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("Serial", "");
            Next(RemoveCartonSerial);
            return true;
        }

        private bool CollectPalletNumber_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("Serial", "");
            Next(RemoveCartonSerial);
            return true;
        }

        private bool _CollectPrinterStartOfOrder_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("Serial", "");
            Next(RemoveCartonSerial);
            return true;
        }

        private bool _PrintBarCodeLabelsStartOfOrder_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("Serial", "");
            Next(RemoveCartonSerial);
            return true;
        }

        #endregion

        #region RemoveCartonSerial
        private void RemoveCartonSerial_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _SerialNumber = e.UserInput.ToString();
            if (_SerialTable.GetSerialNumber(_BranchPlant, _SerialNumber, false))
            {
                if (_SerialTable.LPPLPNU.Trim() != "")
                {
                    ScreenMap.Add("_PalletNumber", _SerialTable.LPPLPNU);
                    ScreenMap.Add("_SerialNumber", _SerialNumber);
                    Next(ConfirmSerialRemove);
                }
                else
                {
                    ScreenMap.Add("Serial", "");
                    throw new RFS_Exception("NOMASTERFOUND");
                }
            }
            else
            {
                ScreenMap.Add("Serial", "");
                throw new RFS_Exception("NOCTNFOUND");
            }
        }

        #endregion

        private void ConfirmSerialRemove_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateRemoveCarton(e.UserInput.ToBool());
        }

        private void validateRemoveCarton(bool confirm)
        {
            if (confirm)
            {
                if (_SerialTable.RemovefromMasterPallet(_BranchPlant, _SerialNumber, _SerialTable.LPPLPNU))
                {
                    ScreenMap.Add("Serial", _SerialNumber + " Removed");
                    Next(RemoveCartonSerial);
                }
                else
                {
                    throw new RFS_Exception("NOTREMOVED");
                }
            }
            else if (!confirm)
            {
                RestartLoop(RemoveCartonSerial);
            }
            else
            {
                throw new RFS_Exception("InvalidEntry");
            }
        }

        private bool CollectCartonSerial_F6Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            PrintMasterCartonLabel();
            RestartLoop(CollectPalletNumber);
            return true;
        }


    } // end StoneridgeMasterPallet Class
} // end ICS.Workflow.Base.JDE.ShopFloor Namespace