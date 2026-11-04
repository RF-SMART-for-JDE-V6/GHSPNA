<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />
  <xsl:template name="RFS_TxCartonMaintenaceDriverBack">
    <xsl:param name="SequenceNumber">
    </xsl:param>
    <Tx Sequence="{$SequenceNumber}" name="RFS_TxCartonMaintenaceDriver" Boundary="1">
      <Fields>
        <Field name="BranchPlant">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szBranchPlant']" />
          </xsl:attribute>
        </Field>
        <Field name="CartonSerialNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szCartonSerialNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="ItemNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szItemNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="LotNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szLotNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="Quantity">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnQuantity']" />
          </xsl:attribute>
        </Field>
        <Field name="UnitOfMeasure">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUnitOfMeasure']" />
          </xsl:attribute>
        </Field>
        <Field name="CompletedCartonID">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnCompletedCartonID']" />
          </xsl:attribute>
        </Field>
        <Field name="FromCartonID">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnFromCartonID']" />
          </xsl:attribute>
        </Field>
        <Field name="ToCartonID">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnToCartonID']" />
          </xsl:attribute>
        </Field>
        <Field name="CartonCode">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szCartonCode']" />
          </xsl:attribute>
        </Field>
        <Field name="CartonJobNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnCartonJobNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="ErrorCode">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'cErrorCode']" />
          </xsl:attribute>
        </Field>
        <Field name="ErrDesc">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szErrDesc']" />
          </xsl:attribute>
        </Field>
        <Field name="P4620Version">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szP4620Version']" />
          </xsl:attribute>
        </Field>
        <Field name="ShipmentNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnShipmentNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="CustomerCartonId">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szCustomerCartonId']" />
          </xsl:attribute>
        </Field>
        <Field name="OrderNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnOrderNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="OrderType">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szOrderType']" />
          </xsl:attribute>
        </Field>
        <Field name="OrderCompany">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szOrderCompany']" />
          </xsl:attribute>
        </Field>
        <Field name="OrderLineNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnOrderLineNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="OrderSuffix">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szOrderSuffix']" />
          </xsl:attribute>
        </Field>
        <Field name="Mode">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'cMode']" />
          </xsl:attribute>
        </Field>
        <Field name="CreateNewCarton">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'cCreateNewCarton']" />
          </xsl:attribute>
        </Field>
        <Field name="SuggestionBatchNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnSuggestionBatchNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="SuggestionSequenceNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnSuggestionSequenceNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="RFUserID">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szRFUserID']" />
          </xsl:attribute>
        </Field>
        <Field name="TransDate_YYYYMMDD">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szTransDate_YYYYMMDD']" />
          </xsl:attribute>
        </Field>
        <Field name="UserInput01">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUserInput01']" />
          </xsl:attribute>
        </Field>
        <Field name="UserInput02">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUserInput02']" />
          </xsl:attribute>
        </Field>
        <Field name="UserInput03">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUserInput03']" />
          </xsl:attribute>
        </Field>
        <Field name="UserInput04">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUserInput04']" />
          </xsl:attribute>
        </Field>
        <Field name="UserInput05">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUserInput05']" />
          </xsl:attribute>
        </Field>
        <Field name="UserInput06">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUserInput06']" />
          </xsl:attribute>
        </Field>
        <Field name="UserInput07">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUserInput07']" />
          </xsl:attribute>
        </Field>
        <Field name="SerialNumberType">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'cSerialNumberType']" />
          </xsl:attribute>
        </Field>
      </Fields>
      <xsl:call-template name="Responses" />
    </Tx>
  </xsl:template>
</xsl:stylesheet>