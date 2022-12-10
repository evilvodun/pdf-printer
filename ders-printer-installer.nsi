!include LogicLib.nsh
!include WinCore.nsh

!ifndef NSIS_CHAR_SIZE
    !define NSIS_CHAR_SIZE 1
!endif

; General Information - prepare files for installation
Outfile "ders-printer.exe"
InstallDir "C:\Program Files"

Section
    SetOutPath $INSTDIR
    File /r "C:\Users\flavi\OneDrive\Desktop\ders-printer\"
SectionEnd

; Add exe directory to the PATH
Section
    Push ${HKEY_CURRENT_USER}
    Push "Environment"
    Push "Path"
    Push ";"
    Push "C:\Program Files\ders-printer"
    Call RegAppendString
    Pop $0
    DetailPrint RegAppendString:Error=$0
SectionEnd 

Function RegAppendString
System::Store S
Pop $R0 ; append
Pop $R1 ; separator
Pop $R2 ; reg value
Pop $R3 ; reg path
Pop $R4 ; reg hkey
System::Call 'ADVAPI32::RegCreateKey(i$R4,tR3,*i.r1)i.r0'
${If} $0 = 0
    System::Call 'ADVAPI32::RegQueryValueEx(ir1,tR2,i0,*i.r2,i0,*i0r3)i.r0'
    ${If} $0 <> 0
        StrCpy $2 ${REG_SZ}
        StrCpy $3 0
    ${EndIf}
    StrLen $4 $R0
    StrLen $5 $R1
    IntOp $4 $4 + $5
    IntOp $4 $4 + 1 ; For \0
    !if ${NSIS_CHAR_SIZE} > 1
        IntOp $4 $4 * ${NSIS_CHAR_SIZE}
    !endif
    IntOp $4 $4 + $3
    System::Alloc $4
    System::Call 'ADVAPI32::RegQueryValueEx(ir1,tR2,i0,i0,isr9,*ir4r4)i.r0'
    ${If} $0 = 0
    ${OrIf} $0 = ${ERROR_FILE_NOT_FOUND}
        System::Call 'KERNEL32::lstrlen(t)(ir9)i.r0'
        ${If} $0 <> 0
            System::Call 'KERNEL32::lstrcat(t)(ir9,tR1)'
        ${EndIf}
        System::Call 'KERNEL32::lstrcat(t)(ir9,tR0)'
        System::Call 'KERNEL32::lstrlen(t)(ir9)i.r0'
        IntOp $0 $0 + 1
        !if ${NSIS_CHAR_SIZE} > 1
            IntOp $0 $0 * ${NSIS_CHAR_SIZE}
        !endif
        System::Call 'ADVAPI32::RegSetValueEx(ir1,tR2,i0,ir2,ir9,ir0)i.r0'
    ${EndIf}
    System::Free $9
    System::Call 'ADVAPI32::RegCloseKey(ir1)'
${EndIf}
Push $0
System::Store L
FunctionEnd
