<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />
  <xsl:template name="RFS_TxCallUBEBack">
    <xsl:param name="SequenceNumber" />
    <Tx Sequence="{$SequenceNumber}" name="RFS_TxCallUBE" Boundary="1">
      <Fields>
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
        <Field name="DataSelection1">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szDataSelection1']" />
          </xsl:attribute>
        </Field>
        <Field name="DataSelection2">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szDataSelection2']" />
          </xsl:attribute>
        </Field>
        <Field name="DataSelection3">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szDataSelection3']" />
          </xsl:attribute>
        </Field>
        <Field name="DataSelection4">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szDataSelection4']" />
          </xsl:attribute>
        </Field>
        <Field name="DataSelection5">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szDataSelection5']" />
          </xsl:attribute>
        </Field>
        <Field name="DataSelection6">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szDataSelection6']" />
          </xsl:attribute>
        </Field>
        <Field name="DataSelection7">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szDataSelection7']" />
          </xsl:attribute>
        </Field>
        <Field name="ErrorCode">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'cErrorCode']" />
          </xsl:attribute>
        </Field>
        <Field name="ErrorDesc">
          <xsl:attribute name="value">
            <xsl:value-of select="params/param[@name = 'szErrorDesc']" />
          </xsl:attribute>
        </Field>
      </Fields>
      <xsl:call-template name="Responses" />
    </Tx>
  </xsl:template>
</xsl:stylesheet>