
/*********************************************************************
 *    Header File:  n59rfcmd.h
 *
 *    Description1:    RFS Carton Maintenance Driver
 *
 *        History:
 *          Date        Programmer  SAR# - Description
 *          ----------  ----------  ----------------------------------
 *   Author 06/01/2017                       - Created
 *
 * Copyright (c) J.D. Edwards World Source Company, 2017
 *
 * This unpublished material is proprietary to J.D.Edwards World Source
 * Company.  All rights reserved.  The methods and techniques described
 * herein are considered trade secrets and/or confidential.  Reproduction
 * or distribution, in whole or in part, is forbidden except by express
 * written permission of J.D. Edwards World Source Company.
 *********************************************************************/

#ifndef __N59RFCMD_H
#define __N59RFCMD_H

/*********************************************************************
 * Table Header Inclusions
 *********************************************************************/

/*********************************************************************
 * External Business Function Header Inclusions
 *********************************************************************/

/*********************************************************************
 * Global Definitions
 *********************************************************************/
#ifdef CER_BOB_MODE
#undef CER_BOB_MODE
#endif
#define CER_BOB_MODE (jdeBitTest(lpObj->dwDialogStatus, \
 STATUS_DATA_COPY)?BOB_MODE_COPY:lpBhvrCom->iBobMode)

/*********************************************************************
 * Structure Definitions
 *********************************************************************/

/*********************************************************************
 * DS Template Type Definitions
 *********************************************************************/

/*********************************************************************
 * TYPEDEF for Data Structure
 *    Template Name: RFS Carton Maintenance Driver
 *    Template ID:   D59RFCMD
 *    Generated:     Thu Jun 01 17:19:49 2017
 *
 * DO NOT EDIT THE FOLLOWING TYPEDEF
 *
 *********************************************************************/

#ifndef DATASTRUCTURE_D59RFCMD
#define DATASTRUCTURE_D59RFCMD

typedef struct tagDSD59RFCMD
{
  JCHAR         szBranchPlant[13];         
  JCHAR         szCartonSerialNumber[41];  
  JCHAR         szItemNumber[26];          
  JCHAR         szLotNumber[31];           
  MATH_NUMERIC  mnQuantity;                
  JCHAR         szUnitOfMeasure[3];        
  MATH_NUMERIC  mnCompletedCartonID;       
  MATH_NUMERIC  mnFromCartonID;            
  MATH_NUMERIC  mnToCartonID;              
  JCHAR         szCartonCode[6];           
  MATH_NUMERIC  mnCartonJobNumber;         
  JCHAR         cErrorCode;                
  JCHAR         szErrDesc[31];             
  JCHAR         szP4620Version[11];        
  MATH_NUMERIC  mnShipmentNumber;          
  JCHAR         szCustomerCartonId[26];    
  MATH_NUMERIC  mnOrderNumber;             
  JCHAR         szOrderType[3];            
  JCHAR         szOrderCompany[6];         
  MATH_NUMERIC  mnOrderLineNumber;         
  JCHAR         szOrderSuffix[4];          
  JCHAR         cMode;                     
  JCHAR         cCreateNewCarton;          
  MATH_NUMERIC  mnSuggestionBatchNumber;   
  MATH_NUMERIC  mnSuggestionSequenceNumber;
  JCHAR         szRFUserID[11];            
  JCHAR         szTransDate_YYYYMMDD[9];   
  JCHAR         szUserInput01[41];         
  JCHAR         szUserInput02[41];         
  JCHAR         szUserInput03[41];         
  JCHAR         szUserInput04[41];         
  JCHAR         szUserInput05[41];         
  JCHAR         szUserInput06[41];         
  JCHAR         szUserInput07[41];         
  JCHAR         cSerialNumberType;         
} DSD59RFCMD , *PDSD59RFCMD , *LPDSD59RFCMD;

#define IDERRszBranchPlant_1                      1L
#define IDERRszCartonSerialNumber_2               2L
#define IDERRszItemNumber_3                       3L
#define IDERRszLotNumber_4                        4L
#define IDERRmnQuantity_5                         5L
#define IDERRszUnitOfMeasure_6                    6L
#define IDERRmnCompletedCartonID_7                7L
#define IDERRmnFromCartonID_8                     8L
#define IDERRmnToCartonID_9                       9L
#define IDERRszCartonCode_10                      10L
#define IDERRmnCartonJobNumber_11                 11L
#define IDERRcErrorCode_12                        12L
#define IDERRszErrDesc_13                         13L
#define IDERRszP4620Version_14                    14L
#define IDERRmnShipmentNumber_15                  15L
#define IDERRszCustomerCartonId_16                16L
#define IDERRmnOrderNumber_17                     17L
#define IDERRszOrderType_18                       18L
#define IDERRszOrderCompany_19                    19L
#define IDERRmnOrderLineNumber_20                 20L
#define IDERRszOrderSuffix_21                     21L
#define IDERRcMode_22                             22L
#define IDERRcCreateNewCarton_23                  23L
#define IDERRmnSuggestionBatchNumber_25           25L
#define IDERRmnSuggestionSequenceNumber_26        26L
#define IDERRszRFUserID_27                        27L
#define IDERRszTransDate_YYYYMMDD_28              28L
#define IDERRszUserInput01_29                     29L
#define IDERRszUserInput02_30                     30L
#define IDERRszUserInput03_31                     31L
#define IDERRszUserInput04_32                     32L
#define IDERRszUserInput05_33                     33L
#define IDERRszUserInput06_34                     34L
#define IDERRszUserInput07_35                     35L
#define IDERRcSerialNumberType_36                 36L

#endif

/*********************************************************************
 * Source Preprocessor Definitions
 *********************************************************************/
#ifdef JDEBFRTN
  #undef JDEBFRTN
#endif
#ifdef WIN32
  #define JDEBFRTN(r) __declspec(dllexport) r
#else
  #define JDEBFRTN(r) r
#endif

/*********************************************************************
 * Business Function Prototypes
 *********************************************************************/

JDEBFRTN(ID) JDEBFWINAPI
RFS_TxCartonMaintenaceDriver( LPBHVRCOM lpBhvrCom ,
    LPVOID lpVoid ,
    LPDSD59RFCMD lpDS );

#endif /* __N59RFCMD_H */
