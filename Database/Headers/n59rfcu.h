
/*********************************************************************
 *    Header File:  n59rfcu.h
 *
 *    Description1:    RFS Call UBE
 *
 *        History:
 *          Date        Programmer  SAR# - Description
 *          ----------  ----------  ----------------------------------
 *   Author 08/01/2023                       - Created
 *
 * Copyright (c) J.D. Edwards World Source Company, 2023
 *
 * This unpublished material is proprietary to J.D.Edwards World Source
 * Company.  All rights reserved.  The methods and techniques described
 * herein are considered trade secrets and/or confidential.  Reproduction
 * or distribution, in whole or in part, is forbidden except by express
 * written permission of J.D. Edwards World Source Company.
 *********************************************************************/

#ifndef __N59RFCU_H
#define __N59RFCU_H

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
 *    Template Name: RFS Call UBE
 *    Template ID:   D59RFCU
 *    Generated:     Tue Aug  1 12:00:38 2023
 *
 * DO NOT EDIT THE FOLLOWING TYPEDEF
 *
 *********************************************************************/

#ifndef DATASTRUCTURE_D59RFCU
#define DATASTRUCTURE_D59RFCU

typedef struct tagDSD59RFCU
{
  JCHAR         szUBEName[11];             
  JCHAR         szUBEVersion[11];          
  JCHAR         szDataSelection1[31];      
  JCHAR         szDataSelection2[31];      
  JCHAR         szDataSelection3[31];      
  JCHAR         szDataSelection4[31];      
  JCHAR         szDataSelection5[31];      
  JCHAR         szDataSelection6[31];      
  JCHAR         szDataSelection7[31];      
  JCHAR         cErrorCode;                
  JCHAR         szErrorDesc[31];           
} DSD59RFCU , *PDSD59RFCU , *LPDSD59RFCU;

#define IDERRszUBEName_1                          1
#define IDERRszUBEVersion_2                       2
#define IDERRszDataSelection1_3                   3
#define IDERRszDataSelection2_4                   4
#define IDERRszDataSelection3_5                   5
#define IDERRszDataSelection4_6                   6
#define IDERRszDataSelection5_7                   7
#define IDERRszDataSelection6_8                   8
#define IDERRszDataSelection7_9                   9
#define IDERRcErrorCode_10                        10
#define IDERRszErrorDesc_12                       12

#endif

/*********************************************************************
 * TYPEDEF for Secondary Data Structure
 *    Template ID:   R553111
 *    Generated:     Tue Aug  1 12:00:38 2023
 *
 * DO NOT EDIT THE FOLLOWING TYPEDEF
 *
 *********************************************************************/

#ifndef CGFORMDSR553111
#define CGFORMDSR553111
typedef struct tagFORMDSR553111
{
  MATH_NUMERIC  mnDocumentOrderInvoiceE;   
  JCHAR         szSerialNumberLot[31];     
} FORMDSR553111 , *PFORMDSR553111 , *LPFORMDSR553111;
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
RFS_TxCallUBE( LPBHVRCOM lpBhvrCom ,
    LPVOID lpVoid ,
    LPDSD59RFCU lpDS );

#endif /* __N59RFCU_H */
