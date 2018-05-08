<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:cs="http://library.by/catalog"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:dt="rss-date-transformer"
                exclude-result-prefixes="msxsl"
                extension-element-prefixes="cs">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/">
    <xsl:element name="rss">
      <xsl:attribute name="version">2.0</xsl:attribute>
      <xsl:element name="channel">
        <xsl:element name="title">Safary Books Online</xsl:element>
        <xsl:element name="link">http://my.safaribooksonline.com/</xsl:element>
        <xsl:element name="description">The latest news from "Safary Books Online", a digital library.</xsl:element>
        <xsl:element name="language">en-us</xsl:element>
        <xsl:apply-templates />
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="cs:book">
    <xsl:element name="item">
      <xsl:apply-templates select="cs:title" />
      <xsl:apply-templates select="cs:description" />
      <xsl:apply-templates select="cs:author" />
      <xsl:apply-templates select="cs:genre" />
      <xsl:apply-templates select="cs:registration_date" />
      <xsl:if test="cs:genre='Science Fiction'">
        <xsl:apply-templates select="cs:isbn" />
      </xsl:if>
    </xsl:element>
  </xsl:template>

  <xsl:template match="cs:book/cs:title">
    <xsl:element name="title">
      <xsl:value-of select="text()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="cs:book/cs:description">
    <xsl:element name="description">
      <xsl:value-of select="text()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="cs:book/cs:genre">
    <xsl:element name="category">
      <xsl:value-of select="text()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="cs:book/cs:registration_date">
    <xsl:element name="pubDate">
      <xsl:value-of select="dt:Transform(text())" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="cs:book/cs:isbn">
    <xsl:element name="link">http://my.safaribooksonline.com/<xsl:value-of select="text()" /></xsl:element>
  </xsl:template>

  <xsl:template match="text() | @*" />
</xsl:stylesheet>
