
/*********************************************************************
 *    Header File:  n59rftsc.h
 *
 *    Description1:    RFS Transportation Shipment Confirm
 *
 *        History:
 *          Date        Programmer  SAR# - Description
 *          ----------  ----------  ----------------------------------
 *   Author 05/24/2017                       - Created
 *
 * Copyright (c) J.D. Edwards World Source Company, 2017
 *
 * This unpublished material is proprietary to J.D.Edwards World Source
 * Company.  All rights reserved.  The methods and techniques described
 * herein are considered trade secrets and/or confidential.  Reproduction
 * or distribution, in whole or in part, is forbidden except by express
 * written permission of J.D. Edwards World Source Company.
 *********************************************************************/

#ifndef __N59RFTSC_H
#define __N59RFTSC_H

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
 *    Template Name: RFS Reroute Shipment
 *    Template ID:   D59RFTSCB
 *    Generated:     Wed May 24 11:15:09 2017
 *
 * DO NOT EDIT THE FOLLOWING TYPEDEF
 *
 *********************************************************************/

#ifndef DATASTRUCTURE_D59RFTSCB
#define DATASTRUCTURE_D59RFTSCB

typedef struct tagDSD59RFTSCB
{
  MATH_NUMERIC  mnShipmentNumber;          
  MATH_NUMERIC  mnRoutingStepNumber;       
  JCHAR         szModeOfTransport[4];      
  MATH_NUMERIC  mnCarrier;                 
  JCHAR         szRFUserId[11];            
  JCHAR         szTransactionDate_YYYYMMDD[9];
  JCHAR         cErrorCode;                
  JCHAR         szErrorDescr[31];          
} DSD59RFTSCB , *PDSD59RFTSCB , *LPDSD59RFTSCB;

#define IDERRmnShipmentNumber_1                   1L
#define IDERRmnRoutingStepNumber_2                2L
#define IDERRszModeOfTransport_3                  3L
#define IDERRmnCarrier_4                          4L
#define IDERRszRFUserId_5                         5L
#define IDERRszTransactionDate_YYYYMMDD_6         6L
#define IDERRcErrorCode_7                         7L
#define IDERRszErrorDescr_8                       8L

#endif

/*********************************************************************
 * TYPEDEF for Data Structure
 *    Template Name: RFS Ship Confirm
 *    Template ID:   D59RFTSCC
 *    Generated:     Wed May 24 11:15:09 2017
 *
 * DO NOT EDIT THE FOLLOWING TYPEDEF
 *
 *********************************************************************/

#ifndef DATASTRUCTURE_D59RFTSCC
#define DATASTRUCTURE_D59RFTSCC

typedef struct tagDSD59RFTSCC
{
  MATH_NUMERIC  mnTransportationJobNumber; 
  JCHAR         szVehRegistration[14];     
  MATH_NUMERIC  mnShipmentNumber;          
  JDEDATE       jdActualShipDate;          
  MATH_NUMERIC  mnActualShipTime;          
  JDEDATE       jdDeliveryDate;            
  JCHAR         cErrorCode;                
  JCHAR         szErrorDescr[31];          
  JCHAR         szP4205Version[11];        
  JCHAR         szRFSUserId[11];           
  JCHAR         szTransactionDate_YYYYMMDD[9];
  JCHAR         szP49640Version[11];       
} DSD59RFTSCC , *PDSD59RFTSCC , *LPDSD59RFTSCC;

#define IDERRmnTransportationJobNumber_1          1L
#define IDERRszVehRegistration_2                  2L
#define IDERRmnShipmentNumber_3                   3L
#define IDERRjdActualShipDate_4                   4L
#define IDERRmnActualShipTime_5                   5L
#define IDERRjdDeliveryDate_6                     6L
#define IDERRcErrorCode_7                         7L
#define IDERRszErrorDescr_8                       8L
#define IDERRszP4205Version_9                     9L
#define IDERRszRFSUserId_10                       10L
#define IDERRszTransactionDate_YYYYMMDD_11        11L
#define IDERRszP49640Version_12                   12L

#endif

/*********************************************************************
 * TYPEDEF for Data Structure
 *    Template Name: RFS Tare Weight Update
 *    Template ID:   D59RFTSCA
 *    Generated:     Wed May 24 11:15:10 2017
 *
 * DO NOT EDIT THE FOLLOWING TYPEDEF
 *
 *********************************************************************/

#ifndef DATASTRUCTURE_D59RFTSCA
#define DATASTRUCTURE_D59RFTSCA

typedef struct tagDSD59RFTSCA
{
  MATH_NUMERIC  mnShipmentNumber;          
  JCHAR         szSSCCNumberTareLevel[19]; 
  JCHAR         szTransactionDate_YYYYMMDD[9];
  JCHAR         szRFUserId[11];            
  JCHAR         cErrorCode;                
  JCHAR         szErrorDescr[31];          
  JCHAR         szUserInput01[31];         
  JCHAR         szUserInput02[31];         
  JCHAR         szUserInput03[31];         
  JCHAR         szUserInput04[31];         
  JCHAR         szUserInput05[31];         
  JCHAR         szUserInput06[31];         
  JCHAR         szUserInput07[31];         
  MATH_NUMERIC  mnTareWeight;              
  JCHAR         szContainerID[21];         
} DSD59RFTSCA , *PDSD59RFTSCA , *LPDSD59RFTSCA;

#define IDERRmnShipmentNumber_1                   1L
#define IDERRszSSCCNumberTareLevel_2              2L
#define IDERRszTransactionDate_YYYYMMDD_4         4L
#define IDERRszRFUserId_5                         5L
#define IDERRcErrorCode_6                         6L
#define IDERRszErrorDescr_7                       7L
#define IDERRszUserInput01_8                      8L
#define IDERRszUserInput02_9                      9L
#define IDERRszUserInput03_10                     10L
#define IDERRszUserInput04_11                     11L
#define IDERRszUserInput05_12                     12L
#define IDERRszUserInput06_13                     13L
#define IDERRszUserInput07_14                     14L
#define IDERRmnTareWeight_15                      15L
#define IDERRszContainerID_16                     16L

#endif

/*********************************************************************
 * TYPEDEF for Data Structure
 *    Template Name: RFS Transportation Shipment Confirm
 *    Template ID:   D59RFTSC
 *    Generated:     Wed May 24 11:15:10 2017
 *
 * DO NOT EDIT THE FOLLOWING TYPEDEF
 *
 *********************************************************************/

#ifndef DATASTRUCTURE_D59RFTSC
#define DATASTRUCTURE_D59RFTSC

typedef struct tagDSD59RFTSC
{
  MATH_NUMERIC  mnShipmentNumber;          
  MATH_NUMERIC  mnActualShipTime;          
  JCHAR         szDeliveryDate_YYYYMMDD[9];
  MATH_NUMERIC  mnOverrideWeight;          
  JCHAR         szWeightUnitOfMeasure[3];  
  JCHAR         szVehRegistration[14];     
  JCHAR         szP49645Version[11];       
  JCHAR         szRFSUserId[11];           
  JCHAR         szTransactionDate_YYYYMMDD[9];
  JCHAR         cErrorCode;                
  JCHAR         szErrorDescr[31];          
  JCHAR         szUserInput01[31];         
  JCHAR         szUserInput02[31];         
  JCHAR         szUserInput03[31];         
  JCHAR         szUserInput04[31];         
  JCHAR         szUserInput05[31];         
  JCHAR         szUserInput06[31];         
  JCHAR         szUserInput07[31];         
  JCHAR         szActualShipDate_YYYYMMDD[9];
  JCHAR         szModeOfTransport[4];      
  JCHAR         szReferenceNumber[31];     
  MATH_NUMERIC  mnAdditionalCost;          
  MATH_NUMERIC  mnCarrier;                 
  JCHAR         szP49640Version[11];       
  JCHAR         cMode;                     
  JCHAR         szBranchPlant[13];         
  JCHAR         szOrderCompany[6];         
  JCHAR         szOrderType[3];            
  MATH_NUMERIC  mnOrderNumber;             
  MATH_NUMERIC  mnLineNumber;              
  JCHAR         szLocation[21];            
  JCHAR         szUOM[3];                  
  JCHAR         szFreightVersion[11];      
  JCHAR         szLot[31];                 
  JCHAR         szLineType[3];             
  MATH_NUMERIC  mnFreightAmount;           
  MATH_NUMERIC  mnLineQty;                 
  JCHAR         szTrackingNumber[31];      
  JCHAR         szRefNumberQualifier[3];   
  JCHAR         szUBEName[11];             
  JCHAR         szUBEVersion[11];          
  JCHAR         szR4981Version[11];        
} DSD59RFTSC , *PDSD59RFTSC , *LPDSD59RFTSC;

#define IDERRmnShipmentNumber_1                   1L
#define IDERRmnActualShipTime_3                   3L
#define IDERRszDeliveryDate_YYYYMMDD_5            5L
#define IDERRmnOverrideWeight_6                   6L
#define IDERRszWeightUnitOfMeasure_7              7L
#define IDERRszVehRegistration_8                  8L
#define IDERRszP49645Version_9                    9L
#define IDERRszRFSUserId_10                       10L
#define IDERRszTransactionDate_YYYYMMDD_11        11L
#define IDERRcErrorCode_12                        12L
#define IDERRszErrorDescr_13                      13L
#define IDERRszUserInput01_14                     14L
#define IDERRszUserInput02_15                     15L
#define IDERRszUserInput03_16                     16L
#define IDERRszUserInput04_17                     17L
#define IDERRszUserInput05_18                     18L
#define IDERRszUserInput06_19                     19L
#define IDERRszUserInput07_20                     20L
#define IDERRszActualShipDate_YYYYMMDD_21         21L
#define IDERRszModeOfTransport_22                 22L
#define IDERRszReferenceNumber_24                 24L
#define IDERRmnAdditionalCost_25                  25L
#define IDERRmnCarrier_26                         26L
#define IDERRszP49640Version_27                   27L
#define IDERRcMode_28                             28L
#define IDERRszBranchPlant_29                     29L
#define IDERRszOrderCompany_30                    30L
#define IDERRszOrderType_31                       31L
#define IDERRmnOrderNumber_32                     32L
#define IDERRmnLineNumber_33                      33L
#define IDERRszLocation_34                        34L
#define IDERRszUOM_35                             35L
#define IDERRszFreightVersion_36                  36L
#define IDERRszLot_37                             37L
#define IDERRszLineType_38                        38L
#define IDERRmnFreightAmount_39                   39L
#define IDERRmnLineQty_40                         40L
#define IDERRszTrackingNumber_41                  41L
#define IDERRszRefNumberQualifier_42              42L
#define IDERRszUBEName_43                         43L
#define IDERRszUBEVersion_44                      44L
#define IDERRszR4981Version_45                    45L

#endif

/*********************************************************************
 * TYPEDEF for Secondary Data Structure
 *    Template ID:   R4981
 *    Generated:     Wed May 24 11:15:12 2017
 *
 * DO NOT EDIT THE FOLLOWING TYPEDEF
 *
 *********************************************************************/

#ifndef CGFORMDSR4981
#define CGFORMDSR4981
typedef struct tagFORMDSR4981
{
  MATH_NUMERIC  mnShipmentNumber;          
  MATH_NUMERIC  mnRoutingStepNumber;       
  JCHAR         szCostCenterTrip[13];      
  MATH_NUMERIC  mnLoadNumber;              
  MATH_NUMERIC  mnDeliveryNumberA;         
  JCHAR         cNoShipOrdStatusUpd;       
} FORMDSR4981 , *PFORMDSR4981 , *LPFORMDSR4981;
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
RFS_RerouteShipment( LPBHVRCOM lpBhvrCom ,
    LPVOID lpVoid ,
    LPDSD59RFTSCB lpDS );

JDEBFRTN(ID) JDEBFWINAPI
RFS_ShipConfirmation( LPBHVRCOM lpBhvrCom ,
    LPVOID lpVoid ,
    LPDSD59RFTSCC lpDS );

JDEBFRTN(ID) JDEBFWINAPI
RFS_TxTareWeightUpdate( LPBHVRCOM lpBhvrCom ,
    LPVOID lpVoid ,
    LPDSD59RFTSCA lpDS );

JDEBFRTN(ID) JDEBFWINAPI
RFS_TxTransShipmentConfirm( LPBHVRCOM lpBhvrCom ,
    LPVOID lpVoid ,
    LPDSD59RFTSC lpDS );

#endif /* __N59RFTSC_H */
