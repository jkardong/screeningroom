echo off
echo Copying source files into temp folder
echo .
mkdir buildfiles\temp
mkdir buildfiles\temp\images
mkdir buildfiles\temp\source
xcopy images buildfiles\temp\images /s
xcopy source buildfiles\temp\source /s
xcopy varsource\devver.vb buildfiles\temp\source /s
xcopy manifest* buildfiles\temp\ /y
echo .
echo Renaming VB files to BRS files
echo .
xcopy buildfiles\temp\source\*.vb buildfiles\temp\source\*.brs
del buildfiles\temp\source\*.vb
echo .
echo Deleting old build
echo .
del builddev.zip
echo .
echo Generating new build
echo .
cd buildfiles\temp
..\zip a ..\..\builddev.zip images source manifest
echo .
echo Reseting prompt
echo .
cd ..
rmdir temp /s/q
cd ..
echo .
echo Done
echo .
