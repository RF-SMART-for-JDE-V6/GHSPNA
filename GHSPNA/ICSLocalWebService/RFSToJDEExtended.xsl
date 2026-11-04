<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="TxRFS_TxCartonMaintenaceDriver.xsl" />
  <xsl:import href="TxRFS_TxTransShipmentConfirm.xsl" />
  <xsl:import href="TxRFS_TxCallUBE.xsl" />
  <xsl:template match="/" name="RFSToJDECustomXSL">
    <xsl:choose>
      <xsl:when test="@name = 'RFS_TxTransShipmentConfirm'">
        <xsl:for-each select="Fields">
          <xsl:call-template name="RFS_TxTransShipmentConfirm" />
        </xsl:for-each>
      </xsl:when>
      <xsl:when test="@name = 'RFS_TxCartonMaintenaceDriver'">
        <xsl:for-each select="Fields">
          <xsl:call-template name="RFS_TxCartonMaintenaceDriver" />
        </xsl:for-each>
      </xsl:when>
      <xsl:when test="@name = 'RFS_TxCallUBE'">
        <xsl:for-each select="Fields">
          <xsl:call-template name="RFS_TxCallUBE" />
        </xsl:for-each>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>