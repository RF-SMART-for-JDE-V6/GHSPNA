/*
    $Archive:   \\filer\rfsow\pvcsproj\RFSmart3\archives\Base\RTTC_Plugins\ICS\Workflow\Base\JDE\ShopFloor\StoneridgeJobNumberCompletion.cs-arc  $
    $Revision: 20304 $
    $Author: sstroble@ICSNT $
    $Date: 2017-03-15 15:03:48 -0600 (Wed, 15 Mar 2017) $
    $Log:   \\filer\rfsow\pvcsproj\RFSmart3\archives\Base\RTTC_Plugins\ICS\Workflow\Base\JDE\ShopFloor\StoneridgeJobNumberCompletion.cs-arc  $
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
 *  BD025864: StoneridgeJobNumberCompletion 
 * 
 *    Rev 1.136   07 Feb 2008 14:57:12   rgreene
 * BD024509: StoneridgeJobNumberCompletions - CollectLicensePlate Screen 
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
 * BD021596: StoneridgeJobNumberCompletion - LicensePlating
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
 * BD011680: F4 not working correctly on StoneridgeJobNumberCompletion, page name CollectPrinter
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
    /// Screen Control Class For: StoneridgeJobNumberCompletion Workflow
    /// </summary>
    public class StoneridgeJobNumberCompletion : ICS.Workflow.Base.WorkflowDesign.Components.RFSWorkflowDesigner
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

        #endregion

        #region Screens

        ICS.Workflow.Base.JDE.Components.BranchPlantScreen _CollectBranch;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLocation;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLot;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLotSerialExpDate;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectGrade;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPotency;

        ICS.Workflow.Base.JDE.Components.QuantityScreen _CollectQuantity;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectQuantityScrapped;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectScrapUnitOfMeasure;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectSerialNumber;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectShiftCode;
        ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen _CollectUnitOfMeasure;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectWorkOrderNumber;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabels;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabelsStartOfOrder;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinter;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinterStartOfOrder;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _VerifyWorkOrderInformation;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _DisplayToleranceWarning;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLicensePlate;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _ConfirmNewLicensePlate;
        // list/lookup screens
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _BranchPlantList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _UserDefinedCodesList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _LocationMasterList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _WorkOrderMasterList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _EmptyLocationsList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _UnitOfMeasureConversionList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _DefinedPrintersTableList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _ItemBalanceList; // item balance list
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _LicensePlateMasterList;

        #endregion

        #region Collect/Work data

        private string _PrinterName;
        private string _ListLookupType; // denotes whether user is looking up "Location" or "Lot" Info
        private string _BranchPlant; // work order Branch
        private double _WorkOrderNumber; // work order number
        private string _DocumentType; // work order type
        private string _ShiftCode; // shift code
        private string _PrimaryItemNumber; // primary item number
        private string _Location; // issue location
        private string _ScreenFormattedLocation;
        private string _LotNumber; // lot/serial number
        private string _LotSerialExpDate; // lot/serial expiration date
        private string _ToLotGrade;
        private string _ToLotPotency;
        private double _TransactionQuantity; // Used to hold the scrapped quantity and UOM converted
        private string _UnitOfMeasure;       // to the primary
        private string _NextSerialNumber;

        private double _CompQtyUnconverted; // Used to hold the completed quantity and UOM entered
        private string _CompUOMUnconverted; // by the user before being converted to primary

        private double _ScrapQuantity;      // Used to hold the scrapped quantity and UOM converted
        private string _ScrapUnitOfMeasure; // to the primary

        private double _ScrapQtyUnconverted; // Used to hold the scrapped quantity and UOM entered
        private string _ScrapUOMUnconverted; // by the user before being converted to primary

        private double _ShortItemNumber; // short item number
        private string _LongItemNumber; // second item number
        private string _ThirdItemNumber; // third item number
        private string _LotStatus; // lot status
        private string _ExpRet;
        private double _OperationSequence;
        private int _ContainerType;
        private double _QuantityPerCarton;
        CustomJDETables.JobNumberCompletion JNComp;
        CustomJDETables.WorkOrderCompletion WOComp;
        private string _PrinterAddress;
        private string _UnitOfMeasureListType;
        private string _IsLocationControlActive; // Y/N flag to denote if whse is using location control
        private int _SerialNumbersScanned; // s/n scanned
        private string _LicensePlate;
        private string _LicensePlateProcessingType = "";
        private string _ShiftCodesSearchType;
        private bool _FromSerialNumberEntry = false;
        private string _LastDateEntered = " ";
        private string _LocationSeparator;
        private string[] _AllowedLotHoldCodes = null; // allowed lot hold codes
        private string[] _AllowedWorkOrderStatusCodes = null;
        private bool _StartOfOrder = false;
        private bool _FromGenLP = false;
        private string _ERPUpdateVersion;
        private bool _LPLicense = false;
        private string[] _CaseUOM;
        private int _NumberOfLabels;
        private int _NumberofJobs;
        private int _NumberofParts;
        private bool _CaptureMasterCarton;
        private ArrayList _JobNumbers;
        private double _ShipTo;
        private double _BillTo;
        #endregion
        protected ICS.Workflow.Base.JDE.Components.QuantityScreen _CollectDualQuantity;
        protected ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen _CollectDualUnitOfMeasure;
        protected ICS.Workflow.Base.JDE.Components.ItemScreen _CollectItemNumber;
        protected GenericScreen CollectCoantainerType;
        protected GenericScreen CollectJobNumber;
        protected GenericScreen CaptureMasterCarton;
        private string[] _LPEDER;


        #region ERPI

        private ICS.ERPI.JDE.WOCompletion _WOCompletionTransaction = new WOCompletion();

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Workflow : StoneridgeJobNumberCompletion
        /// </summary>
        /// <param name="userSession">Contains UserSession info for controling screen display and DAL.</param>
        public StoneridgeJobNumberCompletion(UserSession userSession) : base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        } // end StoneridgeJobNumberCompletion constructor

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

        #region CollectShiftCode

        //*****************************************************************
        // CollectShiftCode
        //*****************************************************************

        #region CollectShiftCode_EnterEvent

        /// <summary>
        /// Enter event for the CollectShiftCode Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectShiftCode_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToString() == "")
            {
                throw new RFS_Exception("BLANKSNOTALLOWED");
            }
            if (_UserDefinedCodesTable.ValidateUDC("06  ", "SH", e.UserInput.ToString().ToUpper()))
            {
                _ShiftCode = _UserDefinedCodesTable.DRKY;
                UserSession.ScreenHandler.ScreenMap.Add("ShiftCodeDescription", _UserDefinedCodesTable.DRDL01);
            } // end if
            else
            {
                Error("SHIFTCODE", e.UserInput.ToString());
            } // end else
        } // end CollectShiftCode_EnterEvent

        #endregion

        #region CollectShiftCode_ListEvent

        /// <summary>
        /// ListEvent for the CollectShiftCode Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectShiftCode_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ShiftCodesSearchType = "Shift";
            _UserDefinedCodesTable.ResetAll();
            _UserDefinedCodesTable.DRSY = "06  ";
            _UserDefinedCodesTable.DRRT = "SH";
            _UserDefinedCodesTable.Fetch();
        } // end CollectShiftCode_ListEvent

        #endregion

        #region UserDefinedCodesList

        //*****************************************************************
        // UserDefinedCodesList
        //*****************************************************************

        #region UserDefinedCodesList_EnterEvent

        /// <summary>
        /// Enter event for the UserDefinedCodesList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserDefinedCodesList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UserDefinedCodesTable.UpdateCurrentRow();
            _FromSerialNumberEntry = false;
            switch (_ShiftCodesSearchType)
            {
                case "Shift":
                    _ShiftCode = _UserDefinedCodesTable.DRKY;
                    if (_ShiftCode == "")
                    {
                        throw new RFS_Exception("BLANKSNOTALLOWED");
                    }
                    ScreenMap.Add("ShiftCode", _ShiftCode);

                    Next(_CollectWorkOrderNumber);
                    break;
                case "LotStatus":
                    _LotStatus = _UserDefinedCodesTable.DRKY;
                    CheckLotProcessType();
                    break;
                case "Grade":
                    _ToLotGrade = _UserDefinedCodesTable.DRKY;
                    ProcessTransaction();
                    break;
            }
        } // end UserDefinedCodesList_EnterEvent

        #endregion

        #endregion

        #endregion

        #region CollectWorkOrderNumber

        //*****************************************************************
        // CollectWorkOrderNumber
        //*****************************************************************

        #region CollectWorkOrderNumber_EnterEvent

        /// <summary>
        /// Enter event for the CollectWorkOrderNumber Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectWorkOrderNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkOrderNumber = e.UserInput.ToDouble();

            // Checking to see if the WorkOrderNumber entered is blank
            if (_WorkOrderNumber.ToString().Length == 0)
            {
                Error("BLANKSNOTALLOWED");
            } // end if

            // BD015237 - Validate the work order number entered against the work order master
            // taking into account the Allowed Status Codes
            else if (_WorkOrderMasterTable.IsWorkOrderValid(_WorkOrderNumber, WFOptions.RetrieveOptionValue("WODocType"), _AllowedWorkOrderStatusCodes))
            {
                // Validating that the Work Order Number entered exists for the specified branch
                if (_WorkOrderMasterTable.WAMMCU.Trim() == _BranchPlant.Trim())
                {
                    _DocumentType = _WorkOrderMasterTable.WADCTO;
                    _WorkOrderNumber = _WorkOrderMasterTable.WADOCO;
                    LoadScreenMap();
                } // end if
                else
                {
                    Error("BranchMismatch");
                } // end else
            } // end else if
            else
            {
                Error("INVALIDWONUMBER", e.UserInput.ToString());
            } // end else
        } // end CollectWorkOrderNumber_EnterEvent

        #endregion

        #region WorkOrderMasterList

        //*****************************************************************
        // WorkOrderMasterList
        //*****************************************************************

        #region WorkOrderMasterList_EnterEvent

        /// <summary>
        /// Enter event for the WorkOrderMasterList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkOrderMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkOrderMasterTable.UpdateCurrentRow();
            _WorkOrderNumber = _WorkOrderMasterTable.WADOCO;
            _DocumentType = _WorkOrderMasterTable.WADCTO;
            LoadScreenMap();
            Next(_VerifyWorkOrderInformation);
        } // end WorkOrderMasterList_EnterEvent

        #endregion

        #endregion

        #endregion

        #region VerifyWorkOrderInformation

        //*****************************************************************
        // VerifyWorkOrderInformation
        //*****************************************************************

        #region VerifyWorkOrderInformation_EnterEvent

        /// <summary>
        /// Enter event for the VerifyWorkOrderInformation Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VerifyWorkOrderInformation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // This screen is a display for verification only
            // User presses ENTER to continue or hits the BACK button
            if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
            {
                ScreenHandler.DefaultText = "1"; // completed quantity is not yet known
            } // end if

        } // end VerifyWorkOrderInformation_EnterEvent

        #endregion

        #endregion

        #region CollectQuantity

        //*****************************************************************
        // CollectQuantity
        //*****************************************************************

        /// <summary>
        /// CollectQuantity: Some Comments
        /// </summary>
        private void CollectQuantity_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            double ConversionFactor = 0;
            if (WFOptions.RetrieveOptionValue("PalletUOM").Trim() != "")
            {
                foreach (string sUOM in _CaseUOM)
                {
                    if (sUOM != "")
                    {
                        ConversionFactor =
                           this._tblUOMConversion.ConvertToPrimaryUOM(_BranchPlant, _ShortItemNumber, WFOptions.RetrieveOptionValue("PalletUOM").Trim(),
                                                                              sUOM);
                        Log.WriteLine(5, "Conversion Factor after call to ConvertToPrimaryUOM = " + ConversionFactor.ToString());
                        Log.WriteLine(5, "Branch/Plant = " + _BranchPlant);
                        Log.WriteLine(5, "Short Item Number = " + _ShortItemNumber.ToString());
                        Log.WriteLine(5, "From U/M = " + WFOptions.RetrieveOptionValue("PalletUOM").Trim());
                        Log.WriteLine(5, "To U/M = " + sUOM.ToString());
                        if (ConversionFactor == 0)
                        {
                            ConversionFactor =
                               _tblUOMConversion2.ConvertToPrimaryUOM(WFOptions.RetrieveOptionValue("PalletUOM").Trim(), sUOM.ToString());
                        }
                        if (ConversionFactor > 0)
                            break;
                    }
                }
            }
            if (ConversionFactor > 0)
                ScreenHandler.DefaultText = ConversionFactor.ToString();
            else
                ScreenHandler.DefaultText = "";
        }


        #region CollectQuantity_EnterEvent

        /// <summary>
        /// Enter event for the CollectQuantity Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectQuantity_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ScrapQuantity = 0;  // reset in case user backs up from scrap screen
            _ScrapUnitOfMeasure = "";

            try
            {
                // Call calculator function
                string CalculatedQuantity = _ItemLocationTable.CalcQty(e.UserInput.ToString().ToUpper());

                // If quantity entered cannot be calculated, throw an error
                if (CalculatedQuantity == null)
                {
                    Error("INVALIDQUANTITY", e.UserInput.ToString());
                } // end if

                // Checking to see if the quantity is negative and that negatives are allowed
                else if ((Convert.ToDecimal(CalculatedQuantity) >= 0) ||
                         WFOptions.GetYesNoWFOptionValue("AllowNegativeCompletion"))
                {
                    _CompQtyUnconverted = _TransactionQuantity = Convert.ToDouble(CalculatedQuantity);
                    ScreenMap.Add("QuantityCompleted", CalculatedQuantity);
                    ScreenHandler.DefaultText =
                                       _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM"));

                    _CollectQuantity.Validate(CalculatedQuantity);

                } // end else if
                else
                {
                    Error("NEGATIVEENTRY", e.UserInput.ToString());
                } // end else
            } // end try
            catch
            {
                Error("INVALIDQUANTITY");
            } // end catch
        } // end CollectQuantity_EnterEvent

        #endregion

        #endregion

        #region CollectUnitOfMeasure

        //*****************************************************************
        // CollectUnitOfMeasure
        //*****************************************************************

        #region CollectUnitOfMeasure_EnterEvent

        /// <summary>
        /// Enter event for the CollectUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectUnitOfMeasure_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            string sDefaultUOM = _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM"));

            _CompUOMUnconverted = e.UserInput.ToString().ToUpper();  // for licensing plating

            // BD015244 - Checking that the defaulted UOM is overrideable and that the UOM entered
            // matches the default.
            if (WFOptions.GetYesNoWFOptionValue("AllowUOMOverride") ||
                (sDefaultUOM == _CompUOMUnconverted))
            {
                ProcessUOMEnter();
            } // end if
            else
            {
                Error("UOMOverride", sDefaultUOM);
            } // end else
        } // end CollectUnitOfMeasure_EnterEvent

        #endregion

        #region ProcessUOMEnter

        /// <summary>
        /// ProcessUOMEnter is used to process the UOM entered by the user.  It performs
        /// UOM conversions if the UOM entered does not match the Item's Primary.  It also
        /// checks tolerance and exceeding the work order ordered quantity.
        /// </summary>
        private void ProcessUOMEnter()
        {
            double ConversionFactor = 0;

            ConversionFactor = _tblUOMConversion.ConvertUnitOfMeasure(_BranchPlant,
                                                                      _ShortItemNumber,
                                                                      _CompUOMUnconverted,
                                                                      _CollectItemNumber.ItemsTable.IMUOM1);
            if (ConversionFactor == 0)
            {
                Error("UNITOFMEASURE", _CompUOMUnconverted);
            } // end if
            else
            {
                // Convert quantity and UOM to primary for JDE update and stored originals for later use
                Log.WriteLine(5, "Transaction Quantity before applying conversion factor = " + _TransactionQuantity.ToString());
                _TransactionQuantity = _CompQtyUnconverted * ConversionFactor;
                Log.WriteLine(5, "Transaction Quantity after applying conversion factor = " + _TransactionQuantity.ToString());

                ScreenMap.Add("UOMComp", _CompUOMUnconverted); // Map the user specified UOM
                _UnitOfMeasure = _CollectItemNumber.ItemsTable.IMUOM1;

                if (WFOptions.RetrieveOptionValue("PalletUOM").Trim().Length > 0)
                {
                    // CM032935:
                    // Now that we have the quantity in the primary UOM, make sure user has
                    //  not picked more than the max quantity on a single pallet.
                    Log.WriteLine(5, "Pallet UOM from workflow options = \"" + WFOptions.RetrieveOptionValue("PalletUOM").Trim() + "\".");
                    double dMaxPalletQty = _tblUOMConversion.ConvertToPrimaryUOM(_BranchPlant,
                                                                                 _ShortItemNumber,
                                                                                 WFOptions.RetrieveOptionValue("PalletUOM").Trim(), 1);
                    if (dMaxPalletQty == 0)
                    {
                        Log.WriteLine(5, "Could not find conversion to primary for UOM specified in workflow option \"PalletUOM\".");
                        Error("UNITOFMEASURE", _UnitOfMeasure);
                        return;
                    }
                    Log.WriteLine(5, "Maximum pallet quantity in primary UOM = " + dMaxPalletQty.ToString() + ".");
                    if (dMaxPalletQty < _TransactionQuantity)
                    {
                        Log.WriteLine(5, "Error in ProcessUOMEnter(): Quantity exceeds maximum pallet quantity.");
                        Error("OverQty", _UnitOfMeasure);
                        return;
                    }
                }

                // BD015262 - If the order is over completed and the workflow is set to check, issue error
                if (OverCompleted(_CompUOMUnconverted, ConversionFactor))
                {
                    Error("OVERCOMPLETE", _UnitOfMeasure);
                } // end if

                // Checking Tolerance
                else if (ExceedsTolerance(_CompUOMUnconverted, ConversionFactor))
                {
                    _UnitOfMeasureListType = "Comp";
                    Next(_DisplayToleranceWarning);
                } // end else if
                else
                {
                    Next(GetNextScreenForCompUOM());
                } // end else

                _CollectUnitOfMeasure.Validate(_UnitOfMeasure);

            } // end else
        } // end ProcessUOMEnter

        #endregion

        #region CollectUnitOfMeasure_ListEvent

        /// <summary>
        /// List event for the CollectUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureListType = "Comp";
            _tblUOMConversion.ResetAll();
            _tblUOMConversion.UMITM = _WorkOrderMasterTable.WAITM;
            _tblUOMConversion.Fetch();
        } // end CollectUnitOfMeasure_ListEvent

        #endregion

        #region UnitOfMeasureList_EnterEvent

        /// <summary>
        /// Enter event for the UnitOfMeasureList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnitOfMeasureList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            string sDefaultUOM = _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM"));

            // user has selected a record from the list
            _tblUOMConversion.UpdateCurrentRow();

            // BD015244 - Checking that the defaulted UOM is overrideable and that the UOM entered
            // matches the default.
            if (WFOptions.GetYesNoWFOptionValue("AllowUOMOverride") ||
                (sDefaultUOM == _tblUOMConversion.UMUM))
            {
                switch (_UnitOfMeasureListType.Trim())
                {
                    case "Comp":
                        _CompUOMUnconverted = _tblUOMConversion.UMUM; // for licensing plating
                        ProcessUOMEnter();
                        break;
                    case "Scrap":
                        //_ScrapUnitOfMeasure = _tblUOMConversion.UMUM;
                        _ScrapUOMUnconverted = _tblUOMConversion.UMUM;
                        ProcessScrapUOMEnter();
                        break;
                    case "Dual":
                        _CollectDualUnitOfMeasure.Validate(_tblUOMConversion.UMUM);
                        break;
                } // end switch


            } // end if
            else
            {
                Error("UOMOverride", sDefaultUOM);
            } // end else
        } // end UnitOfMeasureList_EnterEvent

        #endregion

        #region GetNextScreenForCompUOM

        /// <summary>
        /// Returns the screen to which the user should be directed after a valid completed UOM
        /// is specified.  Valid is defined as not creating Over Complete or outside of the Tolerance.
        /// If the workflow option is not set to check over completion or the user accecpts the
        /// tolerance, then this is also considered valid.
        /// </summary>
        /// <returns>Screen to which the user should be directed after a valid completed UOM.
        /// The return value should most likely be used in a call to Next.</returns>
        private RFSScreenComponent GetNextScreenForCompUOM()
        {
            /* since we are not sending s/n with scrap, this is no longer required 
             * 
           // for serialized items, at this point we need to capture each S/N
           if ((_ItemBranchTable.IBSRCE.Trim().Length > 0) && 
              (Convert.ToInt16(_ItemBranchTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.LotRequired)))
           {
              if (_TransactionQuantity == 0)
              {
                 return (_CollectQuantityScrapped);
              } // end if
              else
              {
                 _ScrapQuantity = 0;
                 _ScrapUnitOfMeasure = "";

                 // If License Plating is Enabled and a license plate item is fetched, go to the
                 // CollectLicensePlate Screen; otherwise go to the CollectLocation screen.
                 if ((UserSession.IsModuleLicensed("JLP")) &&  
                     (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
                 {
                    return (_CollectLicensePlate);
                 } // end if
                 else
                 {
                    return (_CollectLocation);
                 } // end else
              } // end else
           } // end if
           else
           { */
            //return (_CollectQuantityScrapped); 
            return (_CollectDualQuantity);
            //} // end else
        } // end

        #endregion

        #endregion

        #region CollectDualQuantity

        #region CollectDualQuantity_LoadEvent

        private void CollectDualQuantity_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectDualQuantity.Reset();
            _CollectDualUnitOfMeasure.Reset();

            if (!_CollectItemNumber.IsDualQtyRequired)
            {
                _CollectDualQuantity.CancelLoad = true;
                _CollectDualUnitOfMeasure.CancelLoad = true;
                ScreenHandler.ScreenStack.Pop();
                Next(_CollectQuantityScrapped);
            }
            else
            {
                _CollectDualQuantity.CancelLoad = false;
                _CollectDualUnitOfMeasure.CancelLoad = false;
            }
        }

        #endregion

        #endregion

        #region CollectDualUnitOfMeasure

        #region CollectUnitOfMeasure_ListEvent

        /// <summary>
        /// List event for the CollectUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectDualUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureListType = "Dual";
            _tblUOMConversion.ResetAll();
            _tblUOMConversion.UMITM = _WorkOrderMasterTable.WAITM;
            _tblUOMConversion.Fetch();
        } // end CollectUnitOfMeasure_ListEvent

        #endregion

        #endregion

        #region CollectQuantityScrapped

        //*****************************************************************
        // CollectQuantityScrapped
        //*****************************************************************

        #region CollectQuantityScrapped_EnterEvent

        /// <summary>
        /// Enter event for the CollectQuantityScrapped Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectQuantityScrapped_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                // Call calculator function
                string CalculatedQuantity = _ItemLocationTable.CalcQty(e.UserInput.ToString().ToUpper());

                // If quantity entered cannot be calculated, throw an error
                if (CalculatedQuantity == null)
                {
                    Error("INVALIDQUANTITY", e.UserInput.ToString());
                } // end if

                // Checking to see if the quantity is negative and that negatives are allowed
                else if (Convert.ToDecimal(CalculatedQuantity) < 0)
                {
                    Error("NEGATIVEENTRY", e.UserInput.ToString());
                } // end else if
                else
                {
                    // Call calculator function
                    _ScrapQtyUnconverted = _ScrapQuantity = Convert.ToDouble(CalculatedQuantity);
                    ScreenMap.Add("QuantityScrapped", CalculatedQuantity);

                    if (_ScrapQuantity == 0)
                    {
                        // If both the completed quantity and the scrapped quantity are blank,
                        // issue error and return to the completed quantity screen
                        if (_TransactionQuantity == 0)
                        {
                            Error("ZEROQUANTITIES");
                        } // end if
                        else if ((_LPLicense)
                               && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive())
                               && (_ItemUOMProfileTable.ALicenPlateItem(_BranchPlant, _ShortItemNumber, _CompUOMUnconverted)))
                        {
                            Next(_CollectLicensePlate);
                        } // end if
                        else
                        {
                            Next(_CollectLocation);
                        } // end else
                    } // end if
                    else
                    {
                        Next(_CollectScrapUnitOfMeasure,
                             _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM")));
                    } // end else
                } // end else
            } // end try
            catch
            {
                Error("INVALIDQUANTITY");
            } // end catch
        } // end CollectQuantityScrapped_EnterEvent

        #endregion

        #endregion

        #region CollectScrapUnitOfMeasure

        //*****************************************************************
        // CollectScrapUnitOfMeasure
        //*****************************************************************

        #region CollectScrapUnitOfMeasure_EnterEvent

        /// <summary>
        /// Enter event for the CollectScrapUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectScrapUnitOfMeasure_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            string sDefaultUOM = _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM"));

            _ScrapUOMUnconverted = e.UserInput.ToString().ToUpper();

            // BD015244 - Checking that the defaulted UOM is overrideable and that the UOM entered
            // matches the default.
            if (WFOptions.GetYesNoWFOptionValue("AllowUOMOverride") ||
                (sDefaultUOM == _ScrapUOMUnconverted))
            {
                ProcessScrapUOMEnter();
            } // end if
            else
            {
                Error("UOMOverride", sDefaultUOM);
            } // end else
        } // end CollectScrapUnitOfMeasure_EnterEvent

        #endregion

        #region ProcessScrapUOMEnter

        /// <summary>
        /// ProcessScrapUOMEnter is used to process the scrap UOM entered by the user.  It
        /// performs UOM conversions if the UOM entered does not match the Item's Primary.
        /// </summary>
        private void ProcessScrapUOMEnter()
        {
            double ConversionFactor = 0;

            ConversionFactor = _tblUOMConversion.ConvertUnitOfMeasure(_BranchPlant,
                                                                      _ShortItemNumber,
                                                                      _ScrapUOMUnconverted,
                                                                      _CollectItemNumber.ItemsTable.IMUOM1);
            if (ConversionFactor == 0)
            {
                Error("UNITOFMEASURE", _ScrapUOMUnconverted);
            } // end if
            else
            {
                // Convert quantity and UOM to primary for JDE update and stored originals for later use
                Log.WriteLine(5, "Scrapped Quantity before applying conversion factor = " + _ScrapQuantity.ToString());
                _ScrapQuantity = _ScrapQtyUnconverted * ConversionFactor;
                Log.WriteLine(5, "Scrapped Quantity after applying conversion factor = " + _ScrapQuantity.ToString());
                _ScrapUnitOfMeasure = _CollectItemNumber.ItemsTable.IMUOM1;
                ScreenMap.Add("UOMScrap", _ScrapUnitOfMeasure); // Map the UOM it becomes the primary

                // If the order is over completed and the workflow is set to check, issue error
                if (OverCompleted(_ScrapUOMUnconverted, ConversionFactor))
                {
                    Error("OVERCOMPLETE", _ScrapUnitOfMeasure);
                } // end if

                // Checking Tolerance
                // else if (ExceedsTolerance(_ScrapUOMUnconverted, ConversionFactor))
                // {
                //    _UnitOfMeasureListType = "Scrap";
                //    Next(_DisplayToleranceWarning);
                //   } // end else if

                // Checking to see if the user has completed anything for the Work Order.  If they have
                // not, then there is no need to collect a license plate.
                else if (_TransactionQuantity != 0 &&
                        (_LPLicense) &&
                        (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()) &&
                        _ItemUOMProfileTable.ALicenPlateItem(_BranchPlant, _ShortItemNumber, _CompUOMUnconverted))
                {
                    Next(_CollectLicensePlate);
                } // end if
                else
                {
                    Next(_CollectLocation);
                } // end else
            } // end if
        } // end ProcessScrapUOMEnter

        #endregion

        #region CollectScrapUnitOfMeasure_ListEvent

        /// <summary>
        /// List event for the CollectScrapUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectScrapUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureListType = "Scrap";
            _tblUOMConversion.ResetAll();
            _tblUOMConversion.UMITM = _WorkOrderMasterTable.WAITM;
            _tblUOMConversion.Fetch();
        } // end CollectScrapUnitOfMeasure_ListEvent

        #endregion

        #endregion

        #region CollectLocation

        //*****************************************************************
        // CollectLocation
        //*****************************************************************

        #region CollectLocation_EnterEvent

        /// <summary>
        /// Enter event for the CollectLocation Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLocation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _Location = e.UserInput.ToString().ToUpper();
            LocationEnterKey();
        } // end CollectLocation_EnterEvent

        #endregion

        #region LocationEnterKey

        /// <summary>
        /// LocationEnterKey handles the Location entered by the user by means of validation.
        /// </summary>
        private void LocationEnterKey()
        {
            // Checking to see if the Location specified by the user is empty is blanks are not allowed.
            if (WFOptions.GetYesNoWFOptionValue("AllowBlankLocation") || (_Location.Trim().Length != 0))
            {
                _LocationMasterTable.ResetAll();
                _LocationMasterTable.MCU = _CollectBranch.BranchPlantsTable.MCMCU;
                _LocationMasterTable.inLOCN = _Location;
                if (_IsLocationControlActive == "Y")
                {
                    if (_LocationMasterTable.ValidateLocation())
                    {
                        _Location = _LocationMasterTable.stbllocn;
                        _ScreenFormattedLocation = _LocationMasterTable.sscnlocn;
                        ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
                        // will need to retrieve item branch again since it may have been cleared on a lookup
                        _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
                        // IF IBOT1Y = 'Y', prompt for potency
                        if (_CollectItemNumber.ItemBranchesTable.IBOT1Y.ToString().Trim().ToUpper() == "Y")
                            Next(_CollectPotency);
                        else
                        {
                            // otherwise, if IBOT2Y = 'Y', prompt for grade
                            if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                                Next(_CollectGrade);
                            else
                                ProcessTransaction();
                        }
                    } // end if
                    else
                    {
                        Error("LOCATION");
                    } // end else
                } // end if
                else
                {
                    _ScreenFormattedLocation = _Location;
                    ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
                    // will need to retrieve item branch again since it may have been cleared on a lookup
                    _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
                    // IF IBOT1Y = 'Y', prompt for potency
                    if (_CollectItemNumber.ItemBranchesTable.IBOT1Y.ToString().Trim().ToUpper() == "Y")
                        Next(_CollectPotency);
                    else
                    {
                        // otherwise, if IBOT2Y = 'Y', prompt for grade
                        if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                            Next(_CollectGrade);
                        else
                            ProcessTransaction();
                    }
                } // end else
            } // end if
            else
            {
                Error("BLANKSNOTALLOWED");
            } // end else
        } // end LocationEnterKey

        #endregion

        #region CollectLocation_KeyEnter

        /// <summary>
        /// Key event for the CollectLocation Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLocation_KeyEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (e.KeyPressed)
            {
                case ActionKey.F2: // user hits F2 - display empty locations
                    if (e.KeyPressed == ActionKey.F2)
                    {
                        _LocationMasterTable.ShowEmptyLocations(_BranchPlant);
                        _EmptyLocationsList.Show();
                    } // end if
                    break;
                default:
                    Error("JDEWrongKey");
                    break;
            } // end switch
        } // end CollectLocation_KeyEvent

        #endregion

        #region EmptyLocationList_EnterEvent

        private void EmptyLocationList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // User has selected a record from the list
            _LocationMasterTable.UpdateCurrentRow();
            _LocationMasterTable.inLOCN = _LocationMasterTable.LMLOCN;
            _LocationMasterTable.MCU = _CollectBranch.BranchPlantsTable.MCMCU;
            _LocationMasterTable.ResetAll();
            _LocationMasterTable.ValidateLocation();
            _Location = _LocationMasterTable.stbllocn;
            _ScreenFormattedLocation = _LocationMasterTable.sscnlocn;
            ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
            // will need to retrieve item branch again since it may have been cleared on a lookup
            _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
            // IF IBOT1Y = 'Y', prompt for potency
            if (_CollectItemNumber.ItemBranchesTable.IBOT1Y.ToString().Trim().ToUpper() == "Y")
                Next(_CollectPotency);
            else
            {
                // otherwise, if IBOT2Y = 'Y', prompt for grade
                if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                    Next(_CollectGrade);
                else
                    ProcessTransaction();
            }
        } // end EmptyLocationList_EnterEvent

        #endregion

        #region CollectLocation_ListEvent

        /// <summary>
        /// List event for the CollectLocation Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLocation_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ListLookupType = "Location";
            _ItemLocationTable.RetrieveItemLocationRecords(_BranchPlant,
                                                           _ShortItemNumber,
                                                           WFOptions.GetYesNoWFOptionValue("ShowZeroOnHand") ? "Y" : "N");
        } // end CollectLocation_ListEvent

        #endregion

        #endregion

        #region CollectGrade

        #region CollectGrade_EnterEvent

        /// <summary>
        /// Some Comments
        /// </summary>
        private void CollectGrade_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_UserDefinedCodesTable.ValidateUDC("40  ", "LG", e.UserInput.ToString().ToUpper()))
            {
                _ToLotGrade = _UserDefinedCodesTable.DRKY;
                ProcessTransaction();
            } // end if
            else
            {
                Error("Grade", e.UserInput.ToString());
            } // end else
        }

        #endregion

        #region CollectGrade_ListEvent

        /// <summary>
        /// ListEvent for the CollectGrade Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectGrade_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ShiftCodesSearchType = "Grade";
            _UserDefinedCodesTable.ResetAll();
            _UserDefinedCodesTable.DRSY = "40  ";
            _UserDefinedCodesTable.DRRT = "LG";
            _UserDefinedCodesTable.Fetch();
        } // end CollectGrade_ListEvent

        #endregion

        private void CollectGrade_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectLocation);
        }

        #endregion

        #region CollectPotency

        #region CollectPotency_EnterEvent

        /// <summary>
        /// Some Comments
        /// </summary>
        private void CollectPotency_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ToLotPotency = e.UserInput.ToString().ToUpper();
            // otherwise, if IBOT2Y = 'Y', prompt for grade
            if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                Next(_CollectGrade);
            else
                ProcessTransaction();
        }

        #endregion

        private void CollectPotency_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectLocation);
        }

        #endregion

        #region ProcessTransaction

        /// <summary>
        /// Directs the user to the collect lot or serial screens based on the process type.
        /// If the process type is equal to a system generated serial number, then the
        /// transaction is sent to the ERP.  An item with no process type triggers the
        /// transaction to be sent to the ERP as well.
        /// </summary>
        private void ProcessTransaction()
        {
            if (_CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber))
            {
                CheckLotProcessType();
            } // end if
            else
            {
                Error("ITEMBRANCH");
            } // end else
        } // end ProcessTransaction

        #endregion

        #region CollectLicensePlate

        #region CollectLicensePlate_EnterEvent

        /// <summary>
        /// Enter event for the CollectLicensePlate Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLicensePlate_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlate = e.UserInput.ToString().ToUpper();
            LicensePlateEnter();
        } // end CollectLicensePlate_EnterEvent

        #endregion

        #region LicensePlateEnter

        /// <summary>
        /// LicensePlateEnter handles the validation of the License Plate entered
        /// ensuring that it is not a location.  A License Plate should not contain
        /// delimiters like a location would.
        /// </summary>
        private void LicensePlateEnter()
        {
            // If License Plate is blank, collect Location
            if (_LicensePlate.Trim().Length == 0)
            {
                _LicensePlate = "";
            } // end if

            // Check to see if we are scanning a license plate (no delimiters)
            else if (_LicensePlate.IndexOf(_LocationSeparator) == -1)
            {
                //Validate that the work order item/lot is valid on the entered license plate
                //or that the license plate is empty/does not exist
                using (F46L11 tempTbl = new F46L11(UserSession.DataAccess, "TempForValidateLPItemToAdd"))
                {
                    if (!tempTbl.ValidateLPItemToAdd(_LicensePlate, _ShortItemNumber, _LotNumber))
                    {
                        if (!tempTbl.ValidateLPItemToAdd(_LicensePlate, _ShortItemNumber)) Error("ItemNotOnLP");
                        else Error("LotNotOnLP");

                        return;
                    }
                }

                // If a license plate, validate that it exists
                _LicensePlateMasterTable.LMMCU = _BranchPlant;
                _LicensePlateMasterTable.LMLPNU = _LicensePlate;
                if (_LicensePlateMasterTable.Fetch())
                {
                    _LicensePlateProcessingType = "E"; // existing

                    // If the license plate is found, then use that location
                    _Location = _LicensePlateMasterTable.LMLOCN;
                    LocationEnterKey();
                } // end if
                else
                {
                    Next(_ConfirmNewLicensePlate);
                    return;
                } // end else
            } // end else if
            else
            {
                _Location = _LicensePlate;
                _LicensePlate = "";
            } // end else
        } // end LicensePlateEnter

        #endregion

        #region ConfirmNewLicensePlate

        private void ConfirmNewLicensePlate_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                if (!e.UserInput.ToBool())
                {
                    ConfirmNewLicensePlate_BackEvent(sender, e);
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                Error("INVALIDINPUTFORTYPE");
                return;
            }

            _FromGenLP = true;
            _LicensePlateProcessingType = "N"; // New
            Next(_CollectLocation);
        }

        private void ConfirmNewLicensePlate_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Clear("LicensePlate");
            _LicensePlate = string.Empty;
            _FromGenLP = false;
            _LicensePlateProcessingType = string.Empty;

            Back();
        }

        #endregion

        #region CollectLicensePlate_LoadEvent

        private void CollectLicensePlate_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (!_ItemUOMProfileTable.ALicenPlateItem(_BranchPlant, _ShortItemNumber))
            {
                _CollectLicensePlate.CancelLoad = true;
                _LicensePlate = "";
                Next(_CollectLocation);
                return;
            }
            if (WFOptions.GetYesNoWFOptionValue("RFSGenLicensePlateNumber"))
            {
                int NextNumber = 0;
                string FormattedNextNumber = "";

                if (_NextNumberTable.GetNextNumber("LicensePlate", out NextNumber, out FormattedNextNumber))
                {
                    ScreenHandler.DefaultText = FormattedNextNumber.ToString();
                }
                else
                {
                    Log.WriteLine(5, "Could not generate license plate number during CollectLicensePlate screen's Load event.");
                }
            }
        } // end CollectLicensePlate_KeyEvent

        #endregion

        #region CollectLicensePlate_ListEvent

        /// <summary>
        /// List event for the CollectLicensePlate Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLicensePlate_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_LicensePlateMasterTable == null)
                _LicensePlateMasterTable = new F46L10(UserSession.DataAccess, "");

            _LicensePlateMasterTable.ResetAll();
            _LicensePlateMasterTable.LMMCU = _BranchPlant;
            _LicensePlateMasterTable.Fetch();
        } // end CollectLicensePlate_ListEvent

        #endregion

        #region LicensePlateMasterList

        //*****************************************************************
        // LicensePlateMasterList
        //*****************************************************************

        #region LicensePlateMasterList_EnterEvent

        /// <summary>
        /// Enter event for the LicensePlateMasterList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicensePlateMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlateMasterTable.UpdateCurrentRow();
            _LicensePlate = _LicensePlateMasterTable.LMLPNU;
            LicensePlateEnter();
        } // end LicensePlateMasterList_EnterEvent

        #endregion

        #endregion

        #endregion

        #region CheckLotProcessType

        /// <summary>
        /// Used to encapsulate the Lot Process check to determine if the item is lot or serial
        /// controlled.  If it is lot controlled, go to CollectLot.  If it is serial controlled,
        /// go to CollectSerials where the determination will be made to collect serials or
        /// send the transaction to the ERP system to generate the serial number.  If the item is
        /// neither lot or serial controlled, send the transaction to the ERP system.
        /// </summary>
        private void CheckLotProcessType()
        {
            // Check to see if this is a lot/serial controlled item
            if (_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0)
            {
                // Check to see if this is a lot controlled item, and if so, display the lot
                // verification screen
                if ((Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) >= Convert.ToInt16(LotSerialDesignations.LotOptional)) &&
                   (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) <= Convert.ToInt16(LotSerialDesignations.LotRequired)))
                {
                    if (WFOptions.GetYesNoWFOptionValue("DefaultWorkOrderToLot"))
                    {
                        ScreenHandler.DefaultText = _WorkOrderNumber.ToString();
                    } // end if

                    Next(_CollectLot);
                } // end if

                // Serialized Logic
                else if (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) >= Convert.ToInt16(LotSerialDesignations.SerialOptional))
                {
                    CollectSerials();
                } // end else if
            } // end if
            else
            {
                _LotNumber = " ";

                SendToERP();

                if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                {
                    // BD015238 - Set Label quantity to zero if the transaction quantity is negative
                    ScreenHandler.DefaultText = (_CompQtyUnconverted < 0) ? "0" : Math.Ceiling(_CompQtyUnconverted).ToString();
                } // end if

                Next(_PrintBarCodeLabels);
            } // end else
        } // end CheckLotProcessType

        #endregion

        #region CollectLot

        //*****************************************************************
        // CollectLot
        //*****************************************************************

        #region CollectLot_EnterEvent

        /// <summary>
        /// Enter event for the CollectLot Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLot_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotNumber = e.UserInput.ToString().ToUpper();
            ProcessLotNumberEnter();
        } // end CollectLot_EnterEvent

        #endregion

        #region ProcessLotNumberEnter

        /// <summary>
        /// ProcessLotNumberEnter is used to validate that a lot was specified if the
        /// process type (IBSRCE) is equal to three (Assigned Manually).  If the lot
        /// is not found in the F4108, then an Expiration Date must be retrieved,
        /// directs the user to CollectLotSerialExpDate screen.
        /// </summary>
        private void ProcessLotNumberEnter()
        {
            _FromSerialNumberEntry = false;
            ScreenMap.Add("LotNumber", _LotNumber);

            // If a lot is required (process type = 3) and no lot was specified, display
            // an error to the user
            if ((_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0) &&
                (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) == 3) && (_LotNumber.Trim().Length == 0))
            {
                Error("LotRequired");

                return;
            } // end if

            // If the Lot number is not found in the F4108, but a lot was specified and lots are not
            // optional, send the user to the CollectLotSerialExpDate screen
            _ShortItemNumber = _CollectItemNumber.ItemsTable.IMITM;
            if (_LotMasterTable.RetrieveLotMaster(_BranchPlant, _ShortItemNumber, _LotNumber))
            {
                if (_ItemLocationTable.IsLotOnHold(_BranchPlant, _ShortItemNumber, _Location, _LotNumber,
                                                   _AllowedLotHoldCodes))
                {
                    Error("HELDLOT");

                    return;
                } // end if
            } // end if
            else
            {
                if (_LotNumber.Trim().Length > 0)
                {
                    Next(_CollectLotSerialExpDate);

                    return;
                } // end if

                if ((_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0) &&
                   (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) > 0))
                {
                    Next(_CollectLotSerialExpDate);

                    return;
                } // end if
            } // end if

            SendToERP();

            if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
            {
                // BD015238 - Set Label quantity to zero if the transaction quantity is negative
                ScreenHandler.DefaultText = (_CompQtyUnconverted < 0) ? "0" : Math.Ceiling(_CompQtyUnconverted).ToString();
            } // end if

            Next(_PrintBarCodeLabels);
        } // end ProcessLotNumberEnter

        #endregion

        #region CollectLot_ListEvent

        /// <summary>
        /// List event for the CollectLot Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLot_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ListLookupType = "Lot";
            _ItemLocationTable.RetrieveItemSerialRecords(_BranchPlant,
                                                         _CollectItemNumber.ItemsTable.IMITM,
                                                         _Location,
                                                         WFOptions.GetYesNoWFOptionValue("ShowZeroOnHand") ? "Y" : "N",
                                                         _AllowedLotHoldCodes);
        } // end CollectLot_ListEvent

        #endregion

        #endregion

        #region CollectLotSerialExpDate

        /// <summary>
        /// Enter event for the LotSerialExpDate Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotSerialExpDate_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotSerialExpDate = e.UserInput.ToString();
            if (!DateTimeUtils.ValidateAndFormat(ref _LotSerialExpDate,
                                                 WFOptions.RetrieveOptionValue("DateFormat")))
            {
                Error("EXPDATE", e.UserInput.ToString());
                return;
            } // end if

            // if this is a lot process type of 5 or 6 (system assigned serial numbers
            // update all using this date
            switch (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE))
            {
                case 5: // system assigned
                case 6: // system assigned
                    double qtytosend = _TransactionQuantity;

                    for (_SerialNumbersScanned = 0; _SerialNumbersScanned < qtytosend; _SerialNumbersScanned++)
                    {
                        // Convert quantity to primary for JDE update
                        _UnitOfMeasure = _CollectItemNumber.ItemsTable.IMUOM1;
                        _TransactionQuantity = 1;
                        SendToERP();
                        _LotNumber = "";
                        _LotSerialExpDate = "";
                    } // end for

                    ScreenMap.Add("SERIALNUMBERSCAN", _SerialNumbersScanned.ToString());
                    Next(_PrintBarCodeLabels);
                    return;

                    break;

            }
            if ((_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0) &&
               (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.LotRequired)))
            {
                _FromSerialNumberEntry = true;
                SerialNumberEditUpdate();
            } // end else ifelse
            {
                SendToERP();

                if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                {
                    // BD015238 - Set Label quantity to zero if the transaction quantity is negative
                    ScreenHandler.DefaultText = (_CompQtyUnconverted < 0) ? "0" : Math.Ceiling(_CompQtyUnconverted).ToString();
                } // end if

                Next(_PrintBarCodeLabels);
            } // end else
        } // end CollectLotSerialExpDate_EnterEvent

        #endregion

        #region CollectSerialNumber

        //*****************************************************************
        // CollectSerialNumber
        //*****************************************************************

        #region CollectSerialNumber_EnterEvent

        /// <summary>
        /// Enter event for the CollectSerialNumber Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectSerialNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotNumber = e.UserInput.ToString();
            ScreenMap.Add("LotNumber", _LotNumber);

            // If Serial Number does not exist, collect the expiration date
            if (_ItemLocationTable.ValidateItemLot(_BranchPlant, _ShortItemNumber, _LotNumber) == false)
            {
                _ItemLocationTable.LIPQOH = 0;
                Next(_CollectLotSerialExpDate, _LastDateEntered);
            } // end if

            // If the Serial Number exists, and has quantity on hand, display an error message
            else if (_ItemLocationTable.LIPQOH > 0)
            {
                Error("SNEXISTS", e.UserInput.ToString());
            } // end else if
            else
            {
                /* TODO - Change to user array of allowed hold codes
                if ((opts["AllowHeldAdjustments"].ToString() == "N") && (ItemLocation.LILOTS.ToString().Trim().Length > 0))
                   Error("HELDLOT");
                else
                   bContinue = true;
                   */
                SerialNumberEditUpdate();
            } // end else
        } // end CollectSerialNumber_EnterEvent

        #endregion

        #region SerialNumberEditUpdate

        /// <summary>
        /// SerialNumberEditUpdate validates that the serial number is not expired
        /// if expired serials are not allowed; otherwise the serial edit line is
        /// created and sent to the ERP system.
        /// </summary>
        private void SerialNumberEditUpdate()
        {
            if (_LotMasterTable.RetrieveLotMaster(_BranchPlant, _ShortItemNumber, _LotNumber) &&
                (DateTimeUtils.JDEJulian(_LotMasterTable.IOMMEJ) < System.DateTime.Today) &&
                !WFOptions.GetYesNoWFOptionValue("AllowExpiredLotAdj"))
            {
                Error("EXPIREDLOT", _LotNumber);
            } // end if
            else
            {
                _SerialNumbersScanned += 1;
                ScreenMap.Add("SERIALNUMBERSCAN", _SerialNumbersScanned.ToString());
                _FromSerialNumberEntry = true;

                SendToERP();

                if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                {
                    ScreenHandler.DefaultText = "1";
                } // end if

                Next(_PrintBarCodeLabels);
            } // end else
        } // end SerialNumberEditUpdate

        #endregion

        #region CollectSerials

        /// <summary>
        /// Used to encapsulate the logic used to collect serial numbers.  If the process type is
        /// system assigned (5 or 6), then a transaction is sent to the ERP with a quantity of 1
        /// for the total _TransactionQuantity.
        /// </summary>
        private void CollectSerials()
        {
            switch (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE))
            {
                case 5: // system assigned
                case 6: // system assigned
                    Next(_CollectLotSerialExpDate);
                    break;
                case 4: // s/n optional
                case 7: // capture each S/N
                    ScreenMap.Add("SERIALNUMBERSCAN", "0");
                    ScreenMap.Add("QUANTITY", _TransactionQuantity.ToString());
                    Next(_CollectSerialNumber);
                    break;
            } // end switch
        } // end CollectSerials

        #endregion

        #endregion

        #region ItemLocationList

        //*****************************************************************
        // ItemLocationList
        //*****************************************************************

        #region ItemLocationList_EnterEvent

        /// <summary>
        /// Enter event for the ItemLocationList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemLocationList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemLocationTable.UpdateCurrentRow();
            if (_ListLookupType == "Location")
            {
                // user has selected a record from the list
                _LocationMasterTable.ResetAll();
                _LocationMasterTable.inLOCN = _ItemLocationTable.LILOCN;
                _LocationMasterTable.MCU = _CollectBranch.BranchPlantsTable.MCMCU;
                _LocationMasterTable.ValidateLocation();
                _Location = _LocationMasterTable.stbllocn;
                _ScreenFormattedLocation = _LocationMasterTable.sscnlocn;
                ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
                // will need to retrieve item branch again since it may have been cleared on a lookup
                _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
                // IF IBOT1Y = 'Y', prompt for potency
                if (_CollectItemNumber.ItemBranchesTable.IBOT1Y.ToString().Trim().ToUpper() == "Y")
                    Next(_CollectPotency);
                else
                {
                    // otherwise, if IBOT2Y = 'Y', prompt for grade
                    if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                        Next(_CollectGrade);
                    else
                        ProcessTransaction();
                }
            } // end if
            else
            {
                _LotNumber = _ItemLocationTable.LILOTN;
                ProcessLotNumberEnter();
            } // end else
        } // end ItemLocationList_EnterEvent

        #endregion

        #endregion

        #region OverCompleted

        /// <summary>
        /// OverCompleted checks to see if the OverComplete workflow option is set to 'Y', which
        /// indicates that the work order should be checked for over completion.  Over completion
        /// is determined if the scrap plus transaction (completed) quantity is greater
        /// than the difference of the work order's WAUORG, WASOQS, and WASOCN respectively
        /// </summary>
        /// <returns>Returns true if work order has been over completed and the OverComplete
        /// workflow option is set to 'Y'; otherwise, false</returns>
        private bool OverCompleted(string sUOM, double dConvFactor)
        {
            double dTotalTxDocoQty;

            // If the UOM passed matches the document's UOM, do not fetch the UOM conversion as it
            // was already passed in to the method
            if (sUOM != _WorkOrderMasterTable.WAUOM)
            {
                // Converting the total transactional quantity to the document UOM by getting the
                // conversion from document to primary and dividing the result to go back to doc
                // because I figured it may be easier to find a conversion to primary than to doc.
                dConvFactor = _tblUOMConversion.ConvertUnitOfMeasure(_BranchPlant,
                                                                     _ShortItemNumber,
                                                                     _WorkOrderMasterTable.WAUOM,
                                                                     _CollectItemNumber.ItemsTable.IMUOM1);
                if (dConvFactor == 0)
                {
                    // Allow the user to proceed despite not being able to correctly test the quantity
                    // entered by the user for the order.  This may need changing to throw an exception.
                    if (WFOptions.GetYesNoWFOptionValue("AllowOverComplete"))
                    {
                        _ERPUpdateVersion = WFOptions.RetrieveOptionValue("ERPCompleteUpdateVersion").Trim();

                        return (false);
                    } // end if
                    else
                    {
                        throw (new RFS_Exception("Unable to convert the WAUOM to the item's primary\n" +
                                                 "in order to correctly check for over completion.",
                                                 "INVOVERCOMPCONV"));
                    } // end else
                } // end if
            } // end if

            // Converting the Tx Qty, which includes the completed and scrapped quantity in the
            // primary UOM, to the Doco UOM for over-completion check
            dTotalTxDocoQty = (_TransactionQuantity + _ScrapQuantity) / dConvFactor;

            // If this is a partial completion, then use the partial completion WFO version
            _ERPUpdateVersion =
               WFOptions.RetrieveOptionValue((dTotalTxDocoQty < (_WorkOrderMasterTable.WAUORG -
                                                                 _WorkOrderMasterTable.WASOQS -
                                                                 _WorkOrderMasterTable.WASOCN)) ?
                                                "ERPPartialUpdateVersion" :
                                                "ERPCompleteUpdateVersion");

            return (!WFOptions.GetYesNoWFOptionValue("AllowOverComplete") &&
                    dTotalTxDocoQty > (_WorkOrderMasterTable.WAUORG -
                                       _WorkOrderMasterTable.WASOQS -
                                       _WorkOrderMasterTable.WASOCN));
        } // end OverCompleted

        #endregion

        #region Tolerance Logic

        #region ExceedsTolerance

        /// <summary>
        /// Checks to see if the Tolerance set up in the workflow option TolerancePercent
        /// is less than the percentage between the _TransactionQuantity and the Open WorkOrder
        /// Quantity.
        /// </summary>
        /// <param name="sUOM">UOM entered by the user</param>
        /// <param name="dConvFactor">Conversion factor to get to the primary UOM from the
        /// UOM entered by the user</param>
        /// <returns>True is tolerance is exceeded; otherwise, false</returns>
        private bool ExceedsTolerance(string sUOM, double dConvFactor)
        {
            // Tolerance Quantity: Ordered qty * (TolerancePercent / 100)
            double dTolQty = _WorkOrderMasterTable.WAUORG *
                             (WFOptions.GetDoubleWFOptionValue("TolerancePercent") / 100);

            // Open quantity on the work order:  Ordered qty - (completed qty + canceled qty)
            double dWOQtyOpen = _WorkOrderMasterTable.WAUORG - (_WorkOrderMasterTable.WASOQS +
                                                                _WorkOrderMasterTable.WASOCN);

            // Total transaction quantity including the completed and scrapped converted to primary
            double dTotalTxQty = _TransactionQuantity + _ScrapQuantity;

            // If the UOM passed matches the document's UOM, do not fetch the UOM conversion as it
            // was already passed in to the method
            if (sUOM != _WorkOrderMasterTable.WAUOM)
            {
                // Converting the total transactional quantity to the document UOM by getting the
                // conversion from document to primary and dividing the result to go back to doc
                // because I figured it may be easier to find a conversion to primary than to doc.
                dConvFactor = _tblUOMConversion.ConvertUnitOfMeasure(_BranchPlant,
                                                                     _ShortItemNumber,
                                                                     _WorkOrderMasterTable.WAUOM,
                                                                     _CollectItemNumber.ItemsTable.IMUOM1);
                if (dConvFactor == 0)
                {
                    throw (new RFS_Exception("Unable to convert the WAUOM to the item's primary\n" +
                                             "in order to correctly check the tolerance.",
                                             "INVTOLCONV"));
                } // end if
            } // end if

            dTotalTxQty /= dConvFactor; // Converting the Tx Qty to the Document UOM for Tolerance Check

            Log.WriteLine(5, "Tolerance Checking");
            Log.WriteLine(5, "WAUORG = " + _WorkOrderMasterTable.WAUORG.ToString());
            Log.WriteLine(5, "WASOQS = " + _WorkOrderMasterTable.WASOQS.ToString());
            Log.WriteLine(5, "WASOCN = " + _WorkOrderMasterTable.WASOCN.ToString());
            Log.WriteLine(5, "(WAUORG - WASOQS) - WASOCN = WorkOrderQuantityOpen");
            Log.WriteLine(5, "WorkOrderOpenQuantity = " + dWOQtyOpen.ToString());

            return ((dTotalTxQty > (dWOQtyOpen + dTolQty)) || (dTotalTxQty < (dWOQtyOpen - dTolQty)));
        } // end ExceedsTolerance

        #endregion

        #region DisplayToleranceWarning

        //*****************************************************************
        // DisplayToleranceWarning
        //*****************************************************************

        #region DisplayToleranceWarning_EnterEvent

        /// <summary>
        /// Enter event for the DisplayToleranceWarning Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayToleranceWarning_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // If the user accepts the tolerance, determine the next screen; otherwise, always
            // go back to collect the completed quantity
            if (e.UserInput.ToBool())
            {
                // Determine the next screen based on whether or not the user just entered the
                // completed or scrapped data
                if (_UnitOfMeasureListType == "Comp")
                {
                    Next(GetNextScreenForCompUOM());
                } // end if
                else
                {
                    Next(_CollectLocation);
                } //end else
            } // end if
            else
            {
                Back(_CollectQuantity);
            } // end else
        } // end DisplayToleranceWarning_EnterEvent

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

            ScreenMap.Add("WorkOrderNumber", _WorkOrderNumber.ToString());
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
                    Back(_CollectSerialNumber);
                } // end if
                else
                {
                    ClearTransaction();
                    Back(_CollectWorkOrderNumber);
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
            _StartOfOrder = false;
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
                            Next(_CollectShiftCode);
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
                Next(_CollectQuantity);
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
                Back(_CollectSerialNumber);
            } // end else if
            else
            {
                ClearTransaction();
                Back(_CollectWorkOrderNumber);
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
            _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant, _ShortItemNumber);
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
            PrintSystem.ContainerType = "";
            if ((_LPLicense)
                && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LicensePlate", _LicensePlate, false);
            } // end if
              //reset all of the job number label data
            for (int i = 1; i < 65; i++)
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
            */
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SRIPartNumber", JNComp.PartNumber, false);// _CollectItemNumber.ItemsTable.IMLITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartDesc", JNComp.PartDescription, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _TransactionQuantity.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", _NextSerialNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_JobNumber", JNComp.JobNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_WorkOrder", JNComp.WorkOrderNumber, false);
            _SerialTable.GetSerialNumber(_BranchPlant, JNComp.SerialNumber, false);
            if (_SerialTable.LPURCD == "0")
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
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.ToString("HH:mm:ss"), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Shift", _ShiftCode, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_JobSequence", JNComp.JobSequence, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Reprint", "ORIGINAL", false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PrimaryLocation", _ItemLocationTable.LILOCN, false);


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
            PrintSystem.ContainerType = "S";
            if ((_LPLicense)
                && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LicensePlate", _LicensePlate, false);
            } // end if
            int jobcount = 1;
            //reset all of the job number label data
            for (int i = 1; i < 65; i++)
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_JobNumber{i}", "", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartNumber{i}", "", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartQty{i}", "", false);
            }
            _SerialTable.GetSerialNumber(_BranchPlant, _NextSerialNumber, false);
            foreach (DataRow dr in _SerialTable.Rows)
            {
                _SerialTable.CurrentRow = dr;

                Log.Debug($"<***>PartNumber = {_SerialTable.LPFRVNUM}");
                Log.Debug($"<***>JobNUmber = {_SerialTable.LPLITM }");
                Log.Debug($"<***>Quantity = {_SerialTable.LPTQTY.ToString()}");
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_JobNumber{jobcount}", _SerialTable.LPFRVNUM, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartNumber{jobcount}", _SerialTable.LPLITM, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartQty{jobcount}", _SerialTable.LPTQTY.ToString(), false);
                jobcount++;
            }
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", _NextSerialNumber, false);
            //foreach(CustomJDETables.JobNumberCompletion JNComp in _JobNumbers)
            //{
            //    Log.Debug($"<***>PartNumber = {JNComp.PartNumber}");
            //    Log.Debug($"<***>JobNUmber = {JNComp.JobNumber}");
            //    Log.Debug($"<***>Quantity = {JNComp.QuantityShipped}");
            //    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_JobNumber{jobcount}", JNComp.JobNumber, false);
            //    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartNumber{jobcount}", JNComp.PartNumber, false);
            //    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartQty{jobcount}", JNComp.QuantityShipped, false);

            //    jobcount++;
            //}
            if (_SerialTable.LPURCD == "0")
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
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.ToString("HH:mm:ss"), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Shift", _ShiftCode, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Reprint", "ORIGINAL", false);
            _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant, _SerialTable.LPITM);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PrimaryLocation", _ItemLocationTable.LILOCN, false);
            //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SRIPartNumber", JNComp.PartNumber, false);
            //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartDesc", JNComp.PartDescription, false);
            //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _TransactionQuantity.ToString(), false);

            //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_JobNumber", JNComp.JobNumber, false);
            //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_WorkOrder", JNComp.WorkOrderNumber, false);


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
        private void SendToERP()
        {
            Transactions.Add(_WOCompletionTransaction);

            Transactions.Branch = _CollectBranch.BranchPlantsTable.MCMCU.Trim().ToUpperInvariant();
            _WOCompletionTransaction.Initialize();
            setItem();
            setToLocation();
            _WOCompletionTransaction.Branch = _CollectBranch.BranchPlantsTable.MCMCU;
            _WOCompletionTransaction.ShiftCode = _ShiftCode.Trim();
            _WOCompletionTransaction.DocumentNumber = _WorkOrderNumber.ToString();
            _WOCompletionTransaction.DocumentType = _DocumentType;
            _WOCompletionTransaction.QuantityCompleted = _TransactionQuantity.ToString();

            // for serialized items, at this point we need to capture each S/N
            if ((_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0) &&
                (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.LotRequired)))
            {
                _WOCompletionTransaction.QuantityCompleted = "1";
            } // end if

            _WOCompletionTransaction.CompletedUOM = _UnitOfMeasure;
            _WOCompletionTransaction.SecondaryQtyCompleted = _CollectDualQuantity.Quantity.ToString();
            _WOCompletionTransaction.SecondaryCompletedUOM = _CollectDualUnitOfMeasure.UnitOfMeasure;
            _WOCompletionTransaction.QuantityScrapped = _ScrapQuantity.ToString();
            _WOCompletionTransaction.ScrappedUOM = _ScrapUnitOfMeasure;
            _WOCompletionTransaction.Lot = _LotNumber;
            _WOCompletionTransaction.LotStatus = _LotStatus.Trim();
            _WOCompletionTransaction.LotGrade = _ToLotGrade.Trim();
            _WOCompletionTransaction.LotPotency = _ToLotPotency;
            _WOCompletionTransaction.LotExpDate_YYYYMMDD = _LotSerialExpDate;
            _WOCompletionTransaction.RFUserID = UserSession.CurrentUser.LoginName;
            _WOCompletionTransaction.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
            _WOCompletionTransaction.Version = _ERPUpdateVersion.Trim();
            _WOCompletionTransaction.LicensePlateNumber = _LicensePlate;
            _WOCompletionTransaction.NewOrExistingLPN = _LicensePlateProcessingType;

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();

            _LotNumber = _WOCompletionTransaction.ReturnField("Lot", 0, 0);
            _LotSerialExpDate = _WOCompletionTransaction.ReturnField("LotExpDate_YYYYMMDD", 0, 0);
            // clear out scrap in case we are completing serialized inventory
            _ScrapQuantity = 0;
            _ScrapUnitOfMeasure = "";
        } // end SendToERP

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

        public StoneridgeJobNumberCompletion()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._CollectGrade = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._UserDefinedCodesList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectShiftCode = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectCoantainerType = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CaptureMasterCarton = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectJobNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectWorkOrderNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._WorkOrderMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._VerifyWorkOrderInformation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._PrintBarCodeLabelsStartOfOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectUnitOfMeasure = new ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen(this);
            this._CollectBranch = new ICS.Workflow.Base.JDE.Components.BranchPlantScreen(this);
            this._BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectPrinterStartOfOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._DefinedPrintersTableList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectItemNumber = new ICS.Workflow.Base.JDE.Components.ItemScreen(this);
            this._UnitOfMeasureConversionList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectScrapUnitOfMeasure = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectDualQuantity = new ICS.Workflow.Base.JDE.Components.QuantityScreen(this);
            this._CollectDualUnitOfMeasure = new ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen(this);
            this._CollectQuantityScrapped = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectQuantity = new ICS.Workflow.Base.JDE.Components.QuantityScreen(this);
            this._LocationMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._EmptyLocationsList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._ItemBalanceList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectLot = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectPotency = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._DisplayToleranceWarning = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectLotSerialExpDate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._LicensePlateMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectLicensePlate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectSerialNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._PrintBarCodeLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ConfirmNewLicensePlate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // _CollectGrade
            // 
            this._CollectGrade.ListScreen = this._UserDefinedCodesList;
            this._CollectGrade.ListScreenControl = this._UserDefinedCodesList;
            this._CollectGrade.ScreenName = "CollectGrade";
            this._CollectGrade.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectGrade_EnterEvent);
            this._CollectGrade.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectGrade_BackEvent);
            this._CollectGrade.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectGrade_ListEvent);
            // 
            // _UserDefinedCodesList
            // 
            this._UserDefinedCodesList.ListScreenParent = this._CollectGrade;
            this._UserDefinedCodesList.ParentScreen = this._CollectGrade;
            this._UserDefinedCodesList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UserDefinedCodesList_EnterEvent);
            this._UserDefinedCodesList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._UserDefinedCodesList_LoadEvent);
            // 
            // _CollectShiftCode
            // 
            this._CollectShiftCode.ListScreen = this._UserDefinedCodesList;
            this._CollectShiftCode.ListScreenControl = this._UserDefinedCodesList;
            this._CollectShiftCode.NextScreen = this.CollectCoantainerType;
            this._CollectShiftCode.ScreenName = "CollectShiftCode";
            this._CollectShiftCode.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectShiftCode_EnterEvent);
            this._CollectShiftCode.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectShiftCode_ListEvent);
            // 
            // CollectCoantainerType
            // 
            this.CollectCoantainerType.NextScreen = this.CaptureMasterCarton;
            this.CollectCoantainerType.ScreenName = "CollectContainerType";
            this.CollectCoantainerType.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectCoantainerType_EnterValidationEvent);
            // 
            // CaptureMasterCarton
            // 
            this.CaptureMasterCarton.NextScreen = this.CollectJobNumber;
            this.CaptureMasterCarton.ScreenName = "CaptureMasterCarton";
            this.CaptureMasterCarton.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CaptureMasterCarton_EnterValidationEvent);
            // 
            // CollectJobNumber
            // 
            this.CollectJobNumber.BackScreen = this.CaptureMasterCarton;
            this.CollectJobNumber.NextScreen = this.CollectJobNumber;
            this.CollectJobNumber.ScreenName = "CollectJobNumber";
            this.CollectJobNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectJobNumber_EnterValidationEvent);
            this.CollectJobNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectJobNumber_LoadEvent);
            this.CollectJobNumber.F6Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectJobNumber_F6Method);
            // 
            // _CollectWorkOrderNumber
            // 
            this._CollectWorkOrderNumber.ListScreen = this._WorkOrderMasterList;
            this._CollectWorkOrderNumber.ListScreenControl = this._WorkOrderMasterList;
            this._CollectWorkOrderNumber.NextScreen = this._VerifyWorkOrderInformation;
            this._CollectWorkOrderNumber.ScreenName = "CollectWorkOrderNumber";
            this._CollectWorkOrderNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectWorkOrderNumber_EnterEvent);
            // 
            // _WorkOrderMasterList
            // 
            this._WorkOrderMasterList.ListScreenParent = this._CollectWorkOrderNumber;
            this._WorkOrderMasterList.ParentScreen = this._CollectWorkOrderNumber;
            this._WorkOrderMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.WorkOrderMasterList_EnterEvent);
            this._WorkOrderMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._WorkOrderMasterList_LoadEvent);
            // 
            // _VerifyWorkOrderInformation
            // 
            this._VerifyWorkOrderInformation.NextScreen = this._PrintBarCodeLabelsStartOfOrder;
            this._VerifyWorkOrderInformation.ScreenName = "VerifyWorkOrderInformation";
            this._VerifyWorkOrderInformation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.VerifyWorkOrderInformation_EnterEvent);
            // 
            // _PrintBarCodeLabelsStartOfOrder
            // 
            this._PrintBarCodeLabelsStartOfOrder.ScreenName = "PrintBarCodeLabelsStartOfOrder";
            this._PrintBarCodeLabelsStartOfOrder.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabelsStartOfOrder_EnterEvent);
            this._PrintBarCodeLabelsStartOfOrder.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._PrintBarCodeLabelsStartOfOrder_LoadEvent);
            // 
            // _CollectUnitOfMeasure
            // 
            this._CollectUnitOfMeasure.BranchScreen = this._CollectBranch;
            this._CollectUnitOfMeasure.DisableToleranceCheck = true;
            this._CollectUnitOfMeasure.DualQtyScreen = null;
            this._CollectUnitOfMeasure.ItemScreen = this._CollectItemNumber;
            this._CollectUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
            this._CollectUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
            this._CollectUnitOfMeasure.NextScreen = this._CollectDualQuantity;
            this._CollectUnitOfMeasure.OriginalQtyScreen = this._CollectQuantity;
            this._CollectUnitOfMeasure.OriginalUOMScreen = null;
            this._CollectUnitOfMeasure.ScreenName = "CollectUnitofMeasure";
            this._CollectUnitOfMeasure.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectUnitOfMeasure_EnterEvent);
            this._CollectUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectUnitOfMeasure_ListEvent);
            this._CollectUnitOfMeasure.UOMConversionsList.ScreenName = "F41002_List";
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
            this._CollectItemNumber.NextScreen = this._VerifyWorkOrderInformation;
            this._CollectItemNumber.ScreenName = "CollectItemNumber";
            // 
            // _UnitOfMeasureConversionList
            // 
            this._UnitOfMeasureConversionList.ListScreenParent = this._CollectDualUnitOfMeasure;
            this._UnitOfMeasureConversionList.ParentScreen = this._CollectDualUnitOfMeasure;
            this._UnitOfMeasureConversionList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UnitOfMeasureList_EnterEvent);
            this._UnitOfMeasureConversionList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._UnitOfMeasureConversionList_LoadEvent);
            // 
            // _CollectScrapUnitOfMeasure
            // 
            this._CollectScrapUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
            this._CollectScrapUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
            this._CollectScrapUnitOfMeasure.ScreenName = "CollectScrapUnitofMeasure";
            this._CollectScrapUnitOfMeasure.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectScrapUnitOfMeasure_EnterEvent);
            this._CollectScrapUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectScrapUnitOfMeasure_ListEvent);
            // 
            // _CollectDualQuantity
            // 
            this._CollectDualQuantity.IsDualQuantityScreen = true;
            this._CollectDualQuantity.ItemScreen = this._CollectItemNumber;
            this._CollectDualQuantity.NextScreen = this._CollectDualUnitOfMeasure;
            this._CollectDualQuantity.ScreenMapKey = "DualQuantity";
            this._CollectDualQuantity.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDualQuantity_LoadEvent);
            // 
            // _CollectDualUnitOfMeasure
            // 
            this._CollectDualUnitOfMeasure.BranchScreen = this._CollectBranch;
            this._CollectDualUnitOfMeasure.DualQtyScreen = this._CollectDualQuantity;
            this._CollectDualUnitOfMeasure.IsDualUnitOfMeasureScreen = true;
            this._CollectDualUnitOfMeasure.ItemScreen = this._CollectItemNumber;
            this._CollectDualUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
            this._CollectDualUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
            this._CollectDualUnitOfMeasure.NextScreen = this._CollectQuantityScrapped;
            this._CollectDualUnitOfMeasure.OriginalQtyScreen = this._CollectQuantity;
            this._CollectDualUnitOfMeasure.OriginalUOMScreen = this._CollectUnitOfMeasure;
            this._CollectDualUnitOfMeasure.ScreenMapKey = "DualUnitOfMeasure";
            this._CollectDualUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDualUnitOfMeasure_ListEvent);
            this._CollectDualUnitOfMeasure.UOMConversionsList.ScreenName = "F41002_List";
            // 
            // _CollectQuantityScrapped
            // 
            this._CollectQuantityScrapped.ScreenName = "CollectQuantityScrapped";
            this._CollectQuantityScrapped.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectQuantityScrapped_EnterEvent);
            // 
            // _CollectQuantity
            // 
            this._CollectQuantity.ItemScreen = this._CollectItemNumber;
            this._CollectQuantity.NextScreen = this._CollectUnitOfMeasure;
            this._CollectQuantity.ScreenName = "CollectQuantity";
            this._CollectQuantity.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectQuantity_EnterEvent);
            this._CollectQuantity.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectQuantity_LoadEvent);
            // 
            // _LocationMasterList
            // 
            this._LocationMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LocationMasterList_LoadEvent);
            this._LocationMasterList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LocationMasterList_LeavingEvent);
            // 
            // _CollectPrinter
            // 
            this._CollectPrinter.ScreenName = "CollectPrinter";
            this._CollectPrinter.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_EnterEvent);
            this._CollectPrinter.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_ListEvent);
            // 
            // _EmptyLocationsList
            // 
            this._EmptyLocationsList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.EmptyLocationList_EnterEvent);
            this._EmptyLocationsList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._EmptyLocationsList_LoadEvent);
            this._EmptyLocationsList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._EmptyLocationsList_LeavingEvent);
            // 
            // _ItemBalanceList
            // 
            this._ItemBalanceList.ListScreenParent = this._CollectLocation;
            this._ItemBalanceList.ParentScreen = this._CollectLocation;
            this._ItemBalanceList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemLocationList_EnterEvent);
            this._ItemBalanceList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ItemBalanceList_LoadEvent);
            // 
            // _CollectLot
            // 
            this._CollectLot.ListScreen = this._ItemBalanceList;
            this._CollectLot.ListScreenControl = this._ItemBalanceList;
            this._CollectLot.ScreenName = "CollectLot";
            this._CollectLot.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLot_EnterEvent);
            this._CollectLot.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLot_ListEvent);
            this._CollectLot.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectLot_LoadEvent);
            // 
            // _CollectLocation
            // 
            this._CollectLocation.ListScreen = this._ItemBalanceList;
            this._CollectLocation.ListScreenControl = this._ItemBalanceList;
            this._CollectLocation.ScreenName = "CollectLocation";
            this._CollectLocation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_EnterEvent);
            this._CollectLocation.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_ListEvent);
            this._CollectLocation.KeyEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_KeyEvent);
            // 
            // _CollectPotency
            // 
            this._CollectPotency.ScreenName = "CollectPotency";
            this._CollectPotency.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPotency_EnterEvent);
            // 
            // _DisplayToleranceWarning
            // 
            this._DisplayToleranceWarning.ScreenName = "DisplayToleranceWarning";
            this._DisplayToleranceWarning.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.DisplayToleranceWarning_EnterEvent);
            // 
            // _CollectLotSerialExpDate
            // 
            this._CollectLotSerialExpDate.ScreenName = "CollectLotSerialExpDate";
            this._CollectLotSerialExpDate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LotSerialExpDate_EnterEvent);
            this._CollectLotSerialExpDate.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectLotSerialExpDate_LoadEvent);
            // 
            // _LicensePlateMasterList
            // 
            this._LicensePlateMasterList.ListScreenParent = this._CollectLicensePlate;
            this._LicensePlateMasterList.ParentScreen = this._CollectLicensePlate;
            this._LicensePlateMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LicensePlateMasterList_EnterEvent);
            this._LicensePlateMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LicensePlateMasterList_LoadEvent);
            // 
            // _CollectLicensePlate
            // 
            this._CollectLicensePlate.ListScreen = this._LicensePlateMasterList;
            this._CollectLicensePlate.ListScreenControl = this._LicensePlateMasterList;
            this._CollectLicensePlate.NextScreen = this._CollectLocation;
            this._CollectLicensePlate.ScreenName = "CollectLicensePlate";
            this._CollectLicensePlate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_EnterEvent);
            this._CollectLicensePlate.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_ListEvent);
            this._CollectLicensePlate.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_LoadEvent);
            // 
            // _CollectSerialNumber
            // 
            this._CollectSerialNumber.ScreenName = "CollectSerialNumber";
            this._CollectSerialNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSerialNumber_EnterEvent);
            this._CollectSerialNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectSerialNumber_LoadEvent);
            // 
            // _PrintBarCodeLabels
            // 
            this._PrintBarCodeLabels.ScreenName = "PrintBarCodeLabels";
            this._PrintBarCodeLabels.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabels_EnterEvent);
            // 
            // _ConfirmNewLicensePlate
            // 
            this._ConfirmNewLicensePlate.ScreenName = "ConfirmNewLicensePlate";
            this._ConfirmNewLicensePlate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmNewLicensePlate_EnterEvent);
            this._ConfirmNewLicensePlate.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmNewLicensePlate_BackEvent);
            // 
            // StoneridgeJobNumberCompletion
            // 
            this.StartScreen = this._CollectBranch;
            this.WorkflowID = "StoneridgeJobNumberCompletion";

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

        #region CollectJobNumber


        private void CollectJobNumber_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToString().Trim() == "")
            {
                throw new RFS_Exception("BLANKSNOTALLOWED");
            }
            if (e.UserInput.ToString().Trim() == "F6")
            {
                PrintMasterCartonLabel();
                _NextSerialNumber = null;
                Next(CaptureMasterCarton);
            }
            else
            {
                DateTime TransDate = new DateTime();
                if (_BranchPlant.Trim().ToUpper() == "73W01")
                {
                    TransDate = DateTime.Now.AddHours(-2);
                }
                else
                {
                    TransDate = DateTime.Now;
                }
                _NumberofJobs++;
                Log.Debug($"<***> Entering CollectJobNumber Validation");
                JNComp = new CustomJDETables.JobNumberCompletion();
                WOComp = new CustomJDETables.WorkOrderCompletion();
                int NextNumber;
                string stationID = UserSession.ClientInfo.ClientIP;

                stationID = stationID.Replace(".", "");

                if (stationID.Length > 10)
                {
                    stationID = stationID.Substring(stationID.Length - 10);
                }

                if (_SalesOrderDetailTagTable.FetchDocumentNumber(_BranchPlant, e.UserInput.ToString().PadLeft(15, ' ')))
                {
                    //Precheck to make sure all of the Work Orders are valid
                    foreach (DataRow DetailTagRow in _SalesOrderDetailTagTable.Rows)
                    {
                        _SalesOrderDetailTagTable.CurrentRow = DetailTagRow;
                        if (_SalesOrderDetailTable.FetchOrderNumber(_SalesOrderDetailTagTable.UDDOCO, _SalesOrderDetailTagTable.UDDCTO,
                                                                            _SalesOrderDetailTagTable.UDKCOO, _SalesOrderDetailTagTable.UDLNID / 1000,
                                                                            WFOptions.RetrieveOptionValue("SOBeginningStatus"), WFOptions.RetrieveOptionValue("SOEndingStatus")))
                        {
                            if (!_ItemBranchTable.GetMRN(_BranchPlant, _SalesOrderDetailTable.SDLITM))
                            {
                                if (!_WorkOrderMasterTable.FetchWorkOrder(_BranchPlant, WFOptions.RetrieveOptionValue("WODocType"), WFOptions.RetrieveOptionValue("WOBeginningStatus"),
                                                                        WFOptions.RetrieveOptionValue("WOEndingStatus"), _SalesOrderDetailTable.SDLITM,
                                                                        WFOptions.RetrieveOptionValue("WODateLogic")))
                                {
                                    _NumberofJobs--;
                                    throw new RFS_Exception("InvalidWO");
                                }
                            }
                        }
                        else
                        {
                            _NumberofJobs--;
                            throw new RFS_Exception("InvalidSOStatus");
                        }
                    }
                    _SalesOrderDetailTagTable.FetchDocumentNumber(_BranchPlant, e.UserInput.ToString().PadLeft(15, ' '));
                    if (_CaptureMasterCarton)
                    {
                        Log.Debug($"<***> _CaptureMasterCarton = {_CaptureMasterCarton} _NextSerialNumber = ***{_NextSerialNumber}***");
                        if (_NextSerialNumber == null)
                        {
                            _NextNumberTable.GetNextNumber("InternalSerial", out NextNumber, out _NextSerialNumber);
                        }
                    }
                    do
                    {
                        JNComp.JobNumber = _SalesOrderDetailTagTable.UDPSJOBNO;
                        JNComp.JobSequence = _SalesOrderDetailTagTable.UDJOBSQ;
                        JNComp.SalesOrderNumber = _SalesOrderDetailTagTable.UDDOCO.ToString();
                        JNComp.SalesOrderDocumentType = _SalesOrderDetailTagTable.UDDCTO;
                        JNComp.SalesOrderCompany = _SalesOrderDetailTagTable.UDKCOO;
                        JNComp.Line = _SalesOrderDetailTagTable.UDLNID.ToString();
                        JNComp.ReturnableExpendable = _ContainerType.ToString();
                        JNComp.Shift = _ShiftCode.Trim();
                        if (_SalesOrderDetailTable.FetchOrderNumber(_SalesOrderDetailTagTable.UDDOCO, _SalesOrderDetailTagTable.UDDCTO,
                                                                        _SalesOrderDetailTagTable.UDKCOO, _SalesOrderDetailTagTable.UDLNID / 1000,
                                                                        WFOptions.RetrieveOptionValue("SOBeginningStatus"), WFOptions.RetrieveOptionValue("SOEndingStatus")))
                        {

                            if (_CaptureMasterCarton)
                            {
                                Log.Debug($"<***> ShipTo = {_SalesOrderDetailTable.SDSHAN} _BillTo = {_SalesOrderDetailTable.SDAN8}");
                                if (_ShipTo == 0 && _BillTo == 0)
                                {
                                    _BillTo = _SalesOrderDetailTable.SDAN8;
                                    _ShipTo = _SalesOrderDetailTable.SDSHAN;
                                }
                                else
                                {
                                    if (_BillTo != _SalesOrderDetailTable.SDAN8 || _ShipTo != _SalesOrderDetailTable.SDSHAN)
                                    {
                                        _NumberofJobs--;
                                        throw new RFS_Exception("SALESORDERMISMATCH");
                                    }
                                }
                            }
                            JNComp.QuantityShipped = _SalesOrderDetailTable.SDSOQS.ToString();
                            _TransactionQuantity = _SalesOrderDetailTable.SDSOQS;
                            JNComp.SOBranchPlant = _SalesOrderDetailTable.SDMCU;
                            JNComp.SoldTo = _SalesOrderDetailTable.SDAN8.ToString();
                            JNComp.ShipTo = _SalesOrderDetailTable.SDSHAN.ToString();
                            JNComp.PartNumber = _SalesOrderDetailTable.SDLITM;
                            JNComp.PartDescription = _SalesOrderDetailTable.SDDSC1;
                            JNComp.ShortItemNumber = _SalesOrderDetailTable.SDITM.ToString();
                            _ShortItemNumber = _SalesOrderDetailTable.SDITM;
                            JNComp.SOLine = (_SalesOrderDetailTable.SDLNID).ToString();
                            JNComp.UOM = _SalesOrderDetailTable.SDUOM;

                            if (!_ItemBranchTable.GetMRN(_BranchPlant, _SalesOrderDetailTable.SDLITM))
                            {
                                //if (WFOptions.GetYesNoWFOptionValue("AllowOverComplete"))
                                //{

                                double PctOver = 0;
                                if (WFOptions.RetrieveOptionValue("AllowOverComplete") == "Y")
                                {
                                    PctOver = Convert.ToDouble(WFOptions.RetrieveOptionValue("TolerancePercent"));
                                    PctOver = (PctOver + 100) / 100;
                                }
                                //}
                                //else
                                //{
                                //if (_WorkOrderMasterTable.FetchWorkOrder(_BranchPlant, WFOptions.RetrieveOptionValue("WODocType"), WFOptions.RetrieveOptionValue("WOBeginningStatus"),
                                //                                        WFOptions.RetrieveOptionValue("WOEndingStatus"), _SalesOrderDetailTable.SDLITM,
                                //                                        WFOptions.RetrieveOptionValue("WODateLogic"), PctOver))
                                if(GetWorkOrder(PctOver))
                                {
                                    //Check for a valid routing
                                    //if (!_WorkOrderRoutingTable.ValidRouting(_BranchPlant.Trim(), _WorkOrderMasterTable.WADOCO, WFOptions.RetrieveOptionValue("WODocType")))
                                    //{
                                    //    _NumberofJobs--;
                                    //    throw new RFS_Exception("INVALIDROUTING");
                                    //}
                                    if (!_JobNumberTableZfile.CheckSequenceTable(_SalesOrderDetailTagTable.UDPSJOBNO, _SalesOrderDetailTagTable.UDJOBSQ,
                                                                                                               _SalesOrderDetailTable.SDDOCO, _SalesOrderDetailTable.SDDCTO, _SalesOrderDetailTable.SDLNID * 1000))

                                    {

                                        Log.Debug($"<***>NonMRN");
                                        //if (_WorkOrderTimeTableZfile.GetUnProcessedQty(_BranchPlant, _WorkOrderNumber))
                                        //{
                                        //    UnProcessed = _WorkOrderTimeTableZfile.SZSOQS;
                                        //    Log.Debug($"<***> UnProcessed = {UnProcessed}");
                                        //}
                                        //if(_JobNumberTableZfile.GetUnprocessedQty(_BranchPlant, _WorkOrderNumber, WFOptions.RetrieveOptionValue("WODocType")))
                                        //{
                                        //    UnProcessed += _JobNumberTableZfile.SCSOQS;
                                        //    Log.Debug($"<***> UnProcessed F5642001 = {UnProcessed}");
                                        //}
                                        //if (((_WorkOrderMasterTable.WAUORG * PctOver) - _WorkOrderMasterTable.WASOQS - UnProcessed) - 1 < 0)
                                        //{
                                        //    throw new RFS_Exception("TOLERANCEEXCEEDED");
                                        //}

                                        if (!_CaptureMasterCarton)
                                        {
                                            _NextNumberTable.GetNextNumber("InternalSerial", out NextNumber, out _NextSerialNumber);
                                        }
                                        JNComp.SerialNumber = _NextSerialNumber;
                                        JNComp.UserID = UserSession.CurrentUser.LoginName;
                                        JNComp.DeviceID = stationID;
                                        JNComp.ProgramID = "StoneridgeJobNumberCompletion";
                                        JNComp.DateCreated = "1" + TransDate.ToString("yy") + TransDate.DayOfYear.ToString("000");
                                        JNComp.TimeCreated = TransDate.ToString("HHmmss");
                                        JNComp.MRN = "N";
                                        JNComp.WorkOrderNumber = _WorkOrderMasterTable.WADOCO.ToString();
                                        JNComp.WorkOrderDocumentType = _WorkOrderMasterTable.WADCTO;
                                        JNComp.WOBranchPlant = WFOptions.RetrieveOptionValue("WOBranchPlant").PadLeft(12, ' ');
                                        if (!_SerialTable.GetEDER(_BranchPlant, _NextSerialNumber, JNComp.PartNumber))
                                        {
                                            JNComp.EDER = "";
                                        }
                                        else
                                        {
                                            var index = Array.FindIndex(_LPEDER, row => row.Contains(_SerialTable.LPEDER));
                                            index++;
                                            JNComp.EDER = _LPEDER[index];
                                        }
                                        if (_JobNumberTableZfile.SendToBackFlush(JNComp))
                                        {
                                            _SerialTable.InsertSerial(JNComp);
                                        }
                                        else
                                        {
                                            throw new RFS_Exception("BACKFLUSHERROR");
                                        }

                                        if (_CaptureMasterCarton)
                                        {
                                            Log.Debug($"<***>Job: {JNComp.JobNumber}  Part: {JNComp.PartNumber}");
                                            _JobNumbers.Add(JNComp);
                                        }
                                        else
                                        {
                                            //todo update F5642001
                                            //todo upate F5846001
                                            PrintBarCode();
                                        }
                                    }
                                    else
                                    {
                                        _NumberofJobs--;
                                        throw new RFS_Exception("JobAlreadyScanned");
                                    }
                                }

                                else
                                {
                                    _NumberofJobs--;
                                    throw new RFS_Exception("WONotFound");
                                }
                                //}
                            }
                            else
                            {
                                if (!_JobNumberTableZfile.CheckSequenceTable(_SalesOrderDetailTagTable.UDPSJOBNO, _SalesOrderDetailTagTable.UDJOBSQ,
                                                                            _SalesOrderDetailTable.SDDOCO, _SalesOrderDetailTable.SDDCTO, _SalesOrderDetailTable.SDLNID * 1000))
                                /*if (!_JobNumberTableZfile.CheckSequenceTable(JNComp.JobNumber, JNComp.JobSequence,
                                    _SalesOrderDetailTable.SDDOCO, _SalesOrderDetailTable.SDDCTO, double.Parse(JNComp.Line)))*/
                                {
                                    Log.Debug($"<***>MRN");
                                    if (!_CaptureMasterCarton)
                                    {
                                        _NextNumberTable.GetNextNumber("InternalSerial", out NextNumber, out _NextSerialNumber);
                                    }
                                    JNComp.SerialNumber = _NextSerialNumber;
                                    JNComp.UserID = UserSession.CurrentUser.LoginName;
                                    JNComp.DeviceID = stationID;
                                    JNComp.ProgramID = "StoneridgeJobNumberCompletion";
                                    JNComp.DateCreated = "1" + TransDate.ToString("yy") + TransDate.DayOfYear.ToString("000");
                                    JNComp.TimeCreated = TransDate.ToString("HHmmss");
                                    JNComp.MRN = "Y";
                                    JNComp.WorkOrderNumber = "0";
                                    JNComp.WorkOrderDocumentType = "";
                                    JNComp.WOBranchPlant = "";
                                    if (!_JobNumberTableZfile.SendToBackFlush(JNComp))
                                    {
                                        throw new RFS_Exception("BACKFLUSHERROR");
                                    }
                                    if (!_SerialTable.GetEDER(_BranchPlant, _NextSerialNumber, JNComp.PartNumber))
                                    {
                                        JNComp.EDER = "";
                                    }
                                    else
                                    {
                                        var index = Array.FindIndex(_LPEDER, row => row.Contains(_SerialTable.LPEDER));
                                        index++;
                                        JNComp.EDER = _LPEDER[index];
                                    }
                                    _SerialTable.InsertSerial(JNComp);

                                    if (_CaptureMasterCarton)
                                    {
                                        Log.Debug($"<***>Job: {JNComp.JobNumber}  Part: {JNComp.PartNumber}");
                                        _JobNumbers.Add(JNComp);
                                    }
                                    else
                                    {
                                        //todo update F5642001
                                        //todo upate F5846001
                                        PrintBarCode();
                                    }
                                }
                                else
                                {
                                    _NumberofJobs--;
                                    throw new RFS_Exception("JobAlreadyScanned");
                                }
                            }
                        }
                        else
                        {
                            _NumberofJobs--;
                            throw new RFS_Exception("InvalidSOStatus");
                        }
                    } while (_SalesOrderDetailTagTable.MoveNext());
                }
                else
                {
                    _NumberofJobs--;
                    throw new RFS_Exception("JobNumberNotFound");
                }
            }
        }
        private bool GetWorkOrder(double PctOver)
        {
            
            if (_WorkOrderMasterTable.FetchWorkOrder(_BranchPlant, WFOptions.RetrieveOptionValue("WODocType"), WFOptions.RetrieveOptionValue("WOBeginningStatus"),
                                                                        WFOptions.RetrieveOptionValue("WOEndingStatus"), _SalesOrderDetailTable.SDLITM,
                                                                        WFOptions.RetrieveOptionValue("WODateLogic"), PctOver))
            {
                do
                {
                    double UnProcessed = 0;
                    if (_WorkOrderTimeTableZfile.GetUnProcessedQty(_BranchPlant, _WorkOrderMasterTable.WADOCO))
                    {
                        UnProcessed = _WorkOrderTimeTableZfile.SZSOQS;
                        Log.Debug($"<***> UnProcessed = {UnProcessed}");
                    }
                    if (_JobNumberTableZfile.GetUnprocessedQty(_BranchPlant, _WorkOrderMasterTable.WADOCO, WFOptions.RetrieveOptionValue("WODocType")))
                    {
                        UnProcessed += _JobNumberTableZfile.SCSOQS;
                        Log.Debug($"<***> UnProcessed F5642001 = {UnProcessed}");
                    }
                    if (((_WorkOrderMasterTable.WAUORG * PctOver) - _WorkOrderMasterTable.WASOQS - UnProcessed) - 1 >= 0 &&
                        _WorkOrderRoutingTable.ValidRouting(_BranchPlant.Trim(), _WorkOrderMasterTable.WADOCO, WFOptions.RetrieveOptionValue("WODocType")))
                    {
                        return true;
                    }
                } while (_WorkOrderMasterTable.MoveNext());
                return false;
            }
            return false;
        }
        private bool CheckForOvercomplete()
        {
            double UnProcessed = 0;

            double PctOver = Convert.ToDouble(WFOptions.RetrieveOptionValue("TolerancePercent"));
            PctOver = (PctOver + 100) / 100;
            if (_WorkOrderTimeTableZfile.GetUnProcessedQty(_BranchPlant, _WorkOrderNumber))
            {
                UnProcessed = _WorkOrderTimeTableZfile.SZSOQS;
                Log.Debug($"<***> UnProcessed = {UnProcessed}");
            }
            if (_JobNumberTableZfile.GetUnprocessedQty(_BranchPlant, _WorkOrderNumber, WFOptions.RetrieveOptionValue("WODocType")))
            {
                UnProcessed += _JobNumberTableZfile.SCSOQS;
                Log.Debug($"<***> UnProcessed F5642001 = {UnProcessed}");
            }
            if (((_WorkOrderMasterTable.WAUORG * PctOver) - _WorkOrderMasterTable.WASOQS - UnProcessed) - 1 < 0)
            {
                if(_WorkOrderMasterTable.MoveNext())
                {
                    if (CheckForOvercomplete())
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        private CustomJDETables.JobNumberCompletion ProcessJobNumber(CustomJDETables.JobNumberCompletion jn)
        {
            jn.JobNumber = _SalesOrderDetailTagTable.UDPSJOBNO;
            jn.JobSequence = _SalesOrderDetailTagTable.UDJOBSQ;
            jn.SalesOrderNumber = _SalesOrderDetailTagTable.UDDOCO.ToString();
            jn.SalesOrderDocumentType = _SalesOrderDetailTagTable.UDDCTO;
            jn.SalesOrderCompany = _SalesOrderDetailTagTable.UDKCOO;
            jn.Line = _SalesOrderDetailTagTable.UDLNID.ToString();
            jn.ReturnableExpendable = _ContainerType.ToString();
            jn.Shift = _ShiftCode.Trim();
            return jn;
        }
        private bool CollectJobNumber_F6Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            foreach (CustomJDETables.JobNumberCompletion JNComp in _JobNumbers)
            {
                Log.Debug($"<***>F6 Job Number = {JNComp.JobNumber} Part Number = {JNComp.PartNumber} Serial = {JNComp.SerialNumber}");
            }
            PrintMasterCartonLabel();
            _NextSerialNumber = null;
            Next(CaptureMasterCarton);

            return true;
        }

        private void CollectJobNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            //_NumberofJobs = _JobNumbers.Count;
            ScreenMap.Add("_NumberofJobs", _NumberofJobs.ToString());
            _NumberofParts = _JobNumbers.Count;
            ScreenMap.Add("_NumberofParts", _NumberofParts.ToString());

        }
        #endregion

        #endregion

        private void CollectCoantainerType_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (e.UserInput.ToString().ToUpper())
            {
                case "R":
                    _ContainerType = 0;
                    ScreenMap.Add("ContainerType", "Returnable");
                    break;
                case "E":
                    _ContainerType = 1;
                    ScreenMap.Add("ContainerType", "Expendable");
                    break;
                default:
                    throw new RFS_Exception("CONTAINER");
            }

            switch (_ContainerType)
            {
                case 0:
                    _ExpRet = "E";
                    break;
                case 1:
                    _ExpRet = "R";
                    break;
                default:
                    _ExpRet = "";
                    break;
            }
            Log.Debug($"<***> CollectContainer exiting...");
            Next(CaptureMasterCarton);
        }

        private void _PrintBarCodeLabelsStartOfOrder_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WFOptions.RetrieveOptionValue("DeafaultCartonLabelPerCarton");
        }

        private void CaptureMasterCarton_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _JobNumbers = new ArrayList();
            _NextSerialNumber = null;
            switch (e.UserInput.ToString().ToUpper())
            {
                case "Y":
                    _NumberofJobs = 0;
                    _NumberofParts = 0;
                    _BillTo = 0;
                    _ShipTo = 0;
                    _CaptureMasterCarton = true;
                    break;
                case "N":
                    _NumberofJobs = 0;
                    _NumberofParts = 0;
                    _CaptureMasterCarton = false;
                    break;
                default:
                    throw new RFS_Exception("INVALIDSELECTION");

            }
        }


    } // end StoneridgeJobNumberCompletion Class
} // end ICS.Workflow.Base.JDE.ShopFloor Namespace