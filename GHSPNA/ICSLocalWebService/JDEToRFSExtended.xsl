<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="JDERFS_TxCartonMaintenaceDriverBack.xsl" />
  <xsl:import href="JDERFS_TxTransShipmentConfirmBack.xsl" />
  <xsl:import href="JDERFS_TxCallUBEBack.xsl" />
  <xsl:template match="/" name="JDEToRFSCustomXSL">
    <xsl:choose>
      <xsl:when test="@name = 'RFS_TxTransShipmentConfirm'">
        <xsl:call-template name="RFS_TxTransShipmentConfirmBack">
          <xsl:with-param name="SequenceNumber" select="@app" />
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="@name = 'RFS_TxCartonMaintenaceDriver'">
        <xsl:call-template name="RFS_TxCartonMaintenaceDriverBack">
          <xsl:with-param name="SequenceNumber" select="@app" />
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="@name = 'RFS_TxCallUBE'">
        <xsl:call-template name="RFS_TxCallUBEBack">
          <xsl:with-param name="SequenceNumber" select="@app" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="MissingTxBack">
          <xsl:with-param name="SequenceNumber" select="@app">
          </xsl:with-param>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>