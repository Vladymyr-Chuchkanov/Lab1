<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:output method="html"></xsl:output>

<xsl:template match="/">
<html>
<body>
<table border = "1">
<TR>
<th>Автор</th>
<th>Название</th>
<th>Цикл</th>
<th>Жанр</th>
<th>Год издания</th>
</TR>
<xsl:for-each select = "Books/Book">
<tr>
<td>
<xsl:value-of select = "@Author"/>
</td>
<td>
<xsl:value-of select = "@Name"/>
</td>
<td>
<xsl:value-of select = "@Cycle"/>
</td>
<td>
<xsl:value-of select = "@Genre"/>
</td>
<td>
<xsl:value-of select = "@PublishYear"/>
</td>
</tr>
</xsl:for-each>
</table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>


