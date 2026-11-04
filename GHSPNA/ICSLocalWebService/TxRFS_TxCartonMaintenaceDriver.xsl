<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />
  <xsl:template match="Fields" name="RFS_TxCartonMaintenaceDriver">
    <callMethod app="RFSmart" name="RFS_TxCartonMaintenaceDriver" runOnError="no">
      <params>
        <param name="szBranchPlant">
          <xsl:value-of select="Field[@name = 'BranchPlant']/@value" />
        </param>
        <param name="szCartonSerialNumber">
          <xsl:value-of select="Field[@name = 'CartonSerialNumber']/@value" />
        </param>
        <param name="szItemNumber">
          <xsl:value-of select="Field[@name = 'ItemNumber']/@value" />
        </param>
        <param name="szLotNumber">
          <xsl:value-of select="Field[@name = 'LotNumber']/@value" />
        </param>
        <param name="mnQuantity">
          <xsl:value-of select="Field[@name = 'Quantity']/@value" />
        </param>
        <param name="szUnitOfMeasure">
          <xsl:value-of select="Field[@name = 'UnitOfMeasure']/@value" />
        </param>
        <param name="mnCompletedCartonID">
          <xsl:value-of select="Field[@name = 'CompletedCartonID']/@value" />
        </param>
        <param name="mnFromCartonID">
          <xsl:value-of select="Field[@name = 'FromCartonID']/@value" />
        </param>
        <param name="mnToCartonID">
          <xsl:value-of select="Field[@name = 'ToCartonID']/@value" />
        </param>
        <param name="szCartonCode">
          <xsl:value-of select="Field[@name = 'CartonCode']/@value" />
        </param>
        <param name="mnCartonJobNumber">
          <xsl:value-of select="Field[@name = 'CartonJobNumber']/@value" />
        </param>
        <param name="cErrorCode">
          <xsl:value-of select="Field[@name = 'ErrorCode']/@value" />
        </param>
        <param name="szErrDesc">
          <xsl:value-of select="Field[@name = 'ErrDesc']/@value" />
        </param>
        <param name="szP4620Version">
          <xsl:value-of select="Field[@name = 'P4620Version']/@value" />
        </param>
        <param name="mnShipmentNumber">
          <xsl:value-of select="Field[@name = 'ShipmentNumber']/@value" />
        </param>
        <param name="szCustomerCartonId">
          <xsl:value-of select="Field[@name = 'CustomerCartonId']/@value" />
        </param>
        <param name="mnOrderNumber">
          <xsl:value-of select="Field[@name = 'OrderNumber']/@value" />
        </param>
        <param name="szOrderType">
          <xsl:value-of select="Field[@name = 'OrderType']/@value" />
        </param>
        <param name="szOrderCompany">
          <xsl:value-of select="Field[@name = 'OrderCompany']/@value" />
        </param>
        <param name="mnOrderLineNumber">
          <xsl:value-of select="Field[@name = 'OrderLineNumber']/@value" />
        </param>
        <param name="szOrderSuffix">
          <xsl:value-of select="Field[@name = 'OrderSuffix']/@value" />
        </param>
        <param name="cMode">
          <xsl:value-of select="Field[@name = 'Mode']/@value" />
        </param>
        <param name="cCreateNewCarton">
          <xsl:value-of select="Field[@name = 'CreateNewCarton']/@value" />
        </param>
        <param name="mnSuggestionBatchNumber">
          <xsl:value-of select="Field[@name = 'SuggestionBatchNumber']/@value" />
        </param>
        <param name="mnSuggestionSequenceNumber">
          <xsl:value-of select="Field[@name = 'SuggestionSequenceNumber']/@value" />
        </param>
        <param name="szRFUserID">
          <xsl:value-of select="Field[@name = 'RFUserID']/@value" />
        </param>
        <param name="szTransDate_YYYYMMDD">
          <xsl:value-of select="Field[@name = 'TransDate_YYYYMMDD']/@value" />
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
        <param name="cSerialNumberType">
          <xsl:value-of select="Field[@name = 'SerialNumberType']/@value" />
        </param>
      </params>
      <onError abort="yes">
      </onError>
    </callMethod>
  </xsl:template>
</xsl:stylesheet>