<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />
  <xsl:template match="Fields" name="RFS_TxCallUBE">
    <callMethod app="RFSmart" name="RFS_TxCallUBE" runOnError="no">
      <params>
        <param name="szUBEName">
          <xsl:value-of select="Field[@name = 'UBEName']/@value" />
        </param>
        <param name="szUBEVersion">
          <xsl:value-of select="Field[@name = 'UBEVersion']/@value" />
        </param>
        <param name="szDataSelection1">
          <xsl:value-of select="Field[@name = 'DataSelection1']/@value" />
        </param>
        <param name="szDataSelection2">
          <xsl:value-of select="Field[@name = 'DataSelection2']/@value" />
        </param>
        <param name="szDataSelection3">
          <xsl:value-of select="Field[@name = 'DataSelection3']/@value" />
        </param>
        <param name="szDataSelection4">
          <xsl:value-of select="Field[@name = 'DataSelection4']/@value" />
        </param>
        <param name="szDataSelection5">
          <xsl:value-of select="Field[@name = 'DataSelection5']/@value" />
        </param>
        <param name="szDataSelection6">
          <xsl:value-of select="Field[@name = 'DataSelection6']/@value" />
        </param>
        <param name="szDataSelection7">
          <xsl:value-of select="Field[@name = 'DataSelection7']/@value" />
        </param>
        <param name="cErrorCode">
          <xsl:value-of select="Field[@name = 'ErrorCode']/@value" />
        </param>
        <param name="szErrorDesc">
          <xsl:value-of select="Field[@name = 'ErrorDesc']/@value" />
        </param>
      </params>
      <onError abort="yes" />
    </callMethod>
  </xsl:template>
</xsl:stylesheet>