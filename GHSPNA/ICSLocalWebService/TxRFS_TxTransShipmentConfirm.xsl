<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />
  <xsl:template match="Fields" name="RFS_TxTransShipmentConfirm">
    <callMethod app="RFSmart" name="RFS_TxTransShipmentConfirm" runOnError="no">
      <params>
        <param name="mnShipmentNumber">
          <xsl:value-of select="Field[@name = 'ShipmentNumber']/@value" />
        </param>
        <param name="szSSCCNumberTareLevel">
          <xsl:value-of select="Field[@name = 'SSCCNumberTareLevel']/@value" />
        </param>
        <param name="szTransactionDate_YYYYMMDD">
          <xsl:value-of select="Field[@name = 'TransactionDate_YYYYMMDD']/@value" />
        </param>
        <param name="szRFUserId">
          <xsl:value-of select="Field[@name = 'RFUserId']/@value" />
        </param>
        <param name="cErrorCode">
          <xsl:value-of select="Field[@name = 'ErrorCode']/@value" />
        </param>
        <param name="szErrorDescr">
          <xsl:value-of select="Field[@name = 'ErrorDescr']/@value" />
        </param>
        <param name="szUserInput01">
          <xsl:value-of select="Field[@name = 'UserInput01']/@value" />
        </param>
        <param name="szUserInput02">
          <xsl:value-of select="Field[@name = 'UserInput02']/@value" />
        </param>
        <param name="szUserInput03">
          <xsl:value-of select="Field[@name = 'UserInput03']/@value" />
        </param>
        <param name="szUserInput04">
          <xsl:value-of select="Field[@name = 'UserInput04']/@value" />
        </param>
        <param name="szUserInput05">
          <xsl:value-of select="Field[@name = 'UserInput05']/@value" />
        </param>
        <param name="szUserInput06">
          <xsl:value-of select="Field[@name = 'UserInput06']/@value" />
        </param>
        <param name="szUserInput07">
          <xsl:value-of select="Field[@name = 'UserInput07']/@value" />
        </param>
        <param name="mnTareWeight">
          <xsl:value-of select="Field[@name = 'TareWeight']/@value" />
        </param>
        <param name="szContainerID">
          <xsl:value-of select="Field[@name = 'ContainerID']/@value" />
        </param>
        <param name="mnShipmentNumber">
          <xsl:value-of select="Field[@name = 'ShipmentNumber']/@value" />
        </param>
        <param name="mnActualShipTime">
          <xsl:value-of select="Field[@name = 'ActualShipTime']/@value" />
        </param>
        <param name="szDeliveryDate_YYYYMMDD">
          <xsl:value-of select="Field[@name = 'DeliveryDate_YYYYMMDD']/@value" />
        </param>
        <param name="mnOverrideWeight">
          <xsl:value-of select="Field[@name = 'OverrideWeight']/@value" />
        </param>
        <param name="szWeightUnitOfMeasure">
          <xsl:value-of select="Field[@name = 'WeightUnitOfMeasure']/@value" />
        </param>
        <param name="szVehRegistration">
          <xsl:value-of select="Field[@name = 'VehRegistration']/@value" />
        </param>
        <param name="szP49645Version">
          <xsl:value-of select="Field[@name = 'P49645Version']/@value" />
        </param>
        <param name="szRFSUserId">
          <xsl:value-of select="Field[@name = 'RFSUserId']/@value" />
        </param>
        <param name="szTransactionDate_YYYYMMDD">
          <xsl:value-of select="Field[@name = 'TransactionDate_YYYYMMDD']/@value" />
        </param>
        <param name="cErrorCode">
          <xsl:value-of select="Field[@name = 'ErrorCode']/@value" />
        </param>
        <param name="szErrorDescr">
          <xsl:value-of select="Field[@name = 'ErrorDescr']/@value" />
        </param>
        <param name="szUserInput01">
          <xsl:value-of select="Field[@name = 'UserInput01']/@value" />
        </param>
        <param name="szUserInput02">
          <xsl:value-of select="Field[@name = 'UserInput02']/@value" />
        </param>
        <param name="szUserInput03">
          <xsl:value-of select="Field[@name = 'UserInput03']/@value" />
        </param>
        <param name="szUserInput04">
          <xsl:value-of select="Field[@name = 'UserInput04']/@value" />
        </param>
        <param name="szUserInput05">
          <xsl:value-of select="Field[@name = 'UserInput05']/@value" />
        </param>
        <param name="szUserInput06">
          <xsl:value-of select="Field[@name = 'UserInput06']/@value" />
        </param>
        <param name="szUserInput07">
          <xsl:value-of select="Field[@name = 'UserInput07']/@value" />
        </param>
        <param name="szActualShipDate_YYYYMMDD">
          <xsl:value-of select="Field[@name = 'ActualShipDate_YYYYMMDD']/@value" />
        </param>
        <param name="szModeOfTransport">
          <xsl:value-of select="Field[@name = 'ModeOfTransport']/@value" />
        </param>
        <param name="szReferenceNumber">
          <xsl:value-of select="Field[@name = 'ReferenceNumber']/@value" />
        </param>
        <param name="mnAdditionalCost">
          <xsl:value-of select="Field[@name = 'AdditionalCost']/@value" />
        </param>
        <param name="mnCarrier">
          <xsl:value-of select="Field[@name = 'Carrier']/@value" />
        </param>
        <param name="szP49640Version">
          <xsl:value-of select="Field[@name = 'P49640Version']/@value" />
        </param>
        <param name="cMode">
          <xsl:value-of select="Field[@name = 'Mode']/@value" />
        </param>
        <param name="szBranchPlant">
          <xsl:value-of select="Field[@name = 'BranchPlant']/@value" />
        </param>
        <param name="szOrderCompany">
          <xsl:value-of select="Field[@name = 'OrderCompany']/@value" />
        </param>
        <param name="szOrderType">
          <xsl:value-of select="Field[@name = 'OrderType']/@value" />
        </param>
        <param name="mnOrderNumber">
          <xsl:value-of select="Field[@name = 'OrderNumber']/@value" />
        </param>
        <param name="mnLineNumber">
          <xsl:value-of select="Field[@name = 'LineNumber']/@value" />
        </param>
        <param name="szLocation">
          <xsl:value-of select="Field[@name = 'Location']/@value" />
        </param>
        <param name="szUOM">
          <xsl:value-of select="Field[@name = 'UOM']/@value" />
        </param>
        <param name="szFreightVersion">
          <xsl:value-of select="Field[@name = 'FreightVersion']/@value" />
        </param>
        <param name="szLot">
          <xsl:value-of select="Field[@name = 'Lot']/@value" />
        </param>
        <param name="szLineType">
          <xsl:value-of select="Field[@name = 'LineType']/@value" />
        </param>
        <param name="mnFreightAmount">
          <xsl:value-of select="Field[@name = 'FreightAmount']/@value" />
        </param>
        <param name="mnLineQty">
          <xsl:value-of select="Field[@name = 'LineQty']/@value" />
        </param>
        <param name="szTrackingNumber">
          <xsl:value-of select="Field[@name = 'TrackingNumber']/@value" />
        </param>
        <param name="szRefNumberQualifier">
          <xsl:value-of select="Field[@name = 'RefNumberQualifier']/@value" />
        </param>
        <param name="szUBEName">
          <xsl:value-of select="Field[@name = 'UBEName']/@value" />
        </param>
        <param name="szUBEVersion">
          <xsl:value-of select="Field[@name = 'UBEVersion']/@value" />
        </param>
        <param name="szR4981Version">
          <xsl:value-of select="Field[@name = 'R4981Version']/@value" />
        </param>
      </params>
      <onError abort="yes">
      </onError>
    </callMethod>
  </xsl:template>
</xsl:stylesheet>