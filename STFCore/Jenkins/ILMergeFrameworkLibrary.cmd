:: Merge some library files for the STB build.

.\SolutionTestBench\ILMerge.exe /ndebug /wildcards /internalize /targetplatform:"v4, C:\Windows\Microsoft.NET\Framework\v4.0.30319" /out:.\ScalableTestFramework\Binaries\PluginSdk\STF.Framework.dll .\ScalableTestFramework\Binaries\STF.Common\STF.Framework.dll .\ScalableTestFramework\Binaries\STF.Common\Telerik*.dll

XCOPY /S /Y .\ScalableTestFramework\Binaries\PluginSdk\STF.Framework.dll .\STBBuild\Examples\References\
XCOPY /S /Y .\ScalableTestFramework\Binaries\STF.Common\STF.Development.dll .\STBBuild\Examples\References\
