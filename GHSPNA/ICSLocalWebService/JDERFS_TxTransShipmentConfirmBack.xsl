<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />
  <xsl:template name="RFS_TxTransShipmentConfirmBack">
    <xsl:param name="SequenceNumber">
    </xsl:param>
    <Tx Sequence="{$SequenceNumber}" name="RFS_TxTransShipmentConfirm" Boundary="1">
      <Fields>
        <Field name="ShipmentNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnShipmentNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="SSCCNumberTareLevel">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szSSCCNumberTareLevel']" />
          </xsl:attribute>
        </Field>
        <Field name="TransactionDate_YYYYMMDD">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szTransactionDate_YYYYMMDD']" />
          </xsl:attribute>
        </Field>
        <Field name="RFUserId">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szRFUserId']" />
          </xsl:attribute>
        </Field>
        <Field name="ErrorCode">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'cErrorCode']" />
          </xsl:attribute>
        </Field>
        <Field name="ErrorDescr">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szErrorDescr']" />
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
        <Field name="TareWeight">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnTareWeight']" />
          </xsl:attribute>
        </Field>
        <Field name="ContainerID">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szContainerID']" />
          </xsl:attribute>
        </Field>
        <Field name="ShipmentNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnShipmentNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="ActualShipTime">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnActualShipTime']" />
          </xsl:attribute>
        </Field>
        <Field name="DeliveryDate_YYYYMMDD">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szDeliveryDate_YYYYMMDD']" />
          </xsl:attribute>
        </Field>
        <Field name="OverrideWeight">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnOverrideWeight']" />
          </xsl:attribute>
        </Field>
        <Field name="WeightUnitOfMeasure">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szWeightUnitOfMeasure']" />
          </xsl:attribute>
        </Field>
        <Field name="VehRegistration">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szVehRegistration']" />
          </xsl:attribute>
        </Field>
        <Field name="P49645Version">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szP49645Version']" />
          </xsl:attribute>
        </Field>
        <Field name="RFSUserId">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szRFSUserId']" />
          </xsl:attribute>
        </Field>
        <Field name="TransactionDate_YYYYMMDD">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szTransactionDate_YYYYMMDD']" />
          </xsl:attribute>
        </Field>
        <Field name="ErrorCode">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'cErrorCode']" />
          </xsl:attribute>
        </Field>
        <Field name="ErrorDescr">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szErrorDescr']" />
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
        <Field name="ActualShipDate_YYYYMMDD">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szActualShipDate_YYYYMMDD']" />
          </xsl:attribute>
        </Field>
        <Field name="ModeOfTransport">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szModeOfTransport']" />
          </xsl:attribute>
        </Field>
        <Field name="ReferenceNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szReferenceNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="AdditionalCost">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnAdditionalCost']" />
          </xsl:attribute>
        </Field>
        <Field name="Carrier">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnCarrier']" />
          </xsl:attribute>
        </Field>
        <Field name="P49640Version">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szP49640Version']" />
          </xsl:attribute>
        </Field>
        <Field name="Mode">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'cMode']" />
          </xsl:attribute>
        </Field>
        <Field name="BranchPlant">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szBranchPlant']" />
          </xsl:attribute>
        </Field>
        <Field name="OrderCompany">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szOrderCompany']" />
          </xsl:attribute>
        </Field>
        <Field name="OrderType">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szOrderType']" />
          </xsl:attribute>
        </Field>
        <Field name="OrderNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnOrderNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="LineNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnLineNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="Location">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szLocation']" />
          </xsl:attribute>
        </Field>
        <Field name="UOM">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUOM']" />
          </xsl:attribute>
        </Field>
        <Field name="FreightVersion">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szFreightVersion']" />
          </xsl:attribute>
        </Field>
        <Field name="Lot">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szLot']" />
          </xsl:attribute>
        </Field>
        <Field name="LineType">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szLineType']" />
          </xsl:attribute>
        </Field>
        <Field name="FreightAmount">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnFreightAmount']" />
          </xsl:attribute>
        </Field>
        <Field name="LineQty">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'mnLineQty']" />
          </xsl:attribute>
        </Field>
        <Field name="TrackingNumber">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szTrackingNumber']" />
          </xsl:attribute>
        </Field>
        <Field name="RefNumberQualifier">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szRefNumberQualifier']" />
          </xsl:attribute>
        </Field>
        <Field name="UBEName">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUBEName']" />
          </xsl:attribute>
        </Field>
        <Field name="UBEVersion">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szUBEVersion']" />
          </xsl:attribute>
        </Field>
        <Field name="R4981Version">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szR4981Version']" />
          </xsl:attribute>
        </Field>
      </Fields>
      <xsl:call-template name="Responses" />
    </Tx>
  </xsl:template>
</xsl:stylesheet>