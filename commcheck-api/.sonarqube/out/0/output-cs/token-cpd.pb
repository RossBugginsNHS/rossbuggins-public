Ω
G/home/rb/source/rossbuggins-public/commcheck-api/Api/CommAllowedEnum.cs
	namespace 	

CommsCheck
 
; 
public 
enum 
CommAllowedEnum 
{ 
Allowed 
, 
Blocked 
, 
Unknown 
} ˛'
S/home/rb/source/rossbuggins-public/commcheck-api/Api/CommsCheckAnswerResponseDto.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct '
CommsCheckAnswerResponseDto 9
(9 :
string 

ResultId 
, 
string 

RequestString 
, 
CommAllowedEnum 
App 
, 
CommAllowedEnum 
Email 
, 
CommAllowedEnum 
SMS 
, 
CommAllowedEnum		 
Postal		 
,		 
string

 

	AppReason

 
,

 
string 

EmailReason 
, 
string 

	SMSReason 
, 
string 

PostalReason 
) 
{ 
public 

static '
CommsCheckAnswerResponseDto - 
FromCommsCheckAnswer. B
(B C
CommsCheckAnswerC S
answerT Z
)Z [
{ 
return 
new '
CommsCheckAnswerResponseDto .
(. /
answer 
. 
ResultId 
, 
answer 
. 
RequestString  
,  !
GetEnum 
( 
answer 
. 
Outcomes #
,# $
$str% *
)* +
,+ ,
GetEnum 
( 
answer 
. 
Outcomes #
,# $
$str% ,
), -
,- .
GetEnum 
( 
answer 
. 
Outcomes #
,# $
$str% *
)* +
,+ ,
GetEnum 
( 
answer 
. 
Outcomes #
,# $
$str% -
)- .
,. /
	GetReason 
( 
answer 
. 
Outcomes %
,% &
$str' ,
), -
,- .
	GetReason 
( 
answer 
. 
Outcomes %
,% &
$str' .
). /
,/ 0
	GetReason 
( 
answer 
. 
Outcomes %
,% &
$str' ,
), -
,- .
	GetReason 
( 
answer 
. 
Outcomes %
,% &
$str' /
)/ 0
) 
; 
} 
private 
static 
CommAllowedEnum "
GetEnum# *
(* +
IEnumerable+ 6
<6 7
IRuleOutcome7 C
>C D
outcomesE M
,M N
stringO U
methodV \
)\ ]
{   
var!! 
outCome!! 
=!! 
outcomes!! 
.!! 
Where!! $
(!!$ %
x!!% &
=>!!& (
x!!) *
.!!* +
Method!!+ 1
==!!2 4
method!!5 ;
)!!; <
.!!< =
FirstOrDefault!!= K
(!!K L
)!!L M
;!!M N
if"" 

(""
 
outCome"" 
=="" 
null"" 
||"" 
outCome"" #
==""$ &
default""' .
)"". /
{## 	
return$$ 
CommAllowedEnum$$ "
.$$" #
Unknown$$# *
;$$* +
}%% 	
else&& 
if&& 
(&& 
outCome&& 
.&& 
	IsAllowed&& !
(&&! "
)&&" #
)&&# $
{'' 	
return(( 
CommAllowedEnum(( "
.((" #
Allowed((# *
;((* +
})) 	
else** 
{++ 	
return,, 
CommAllowedEnum,, "
.,," #
Blocked,,# *
;,,* +
}-- 	
}.. 
private00 
static00 
string00 
	GetReason00 #
(00# $
IEnumerable00$ /
<00/ 0
IRuleOutcome000 <
>00< =
outcomes00> F
,00F G
string00H N
method00O U
)00U V
{11 
var22 
outCome22 
=22 
outcomes22 
.22 
Where22 $
(22$ %
x22% &
=>22& (
x22) *
.22* +
Method22+ 1
==222 4
method225 ;
)22; <
.22< =
FirstOrDefault22= K
(22K L
)22L M
;22M N
if33 

(33
 
outCome33 
==33 
null33 
||33 
outCome33 #
==33$ &
default33' .
)33. /
{44 	
return55 
string55 
.55 
Empty55 
;55  
}66 	
else77 
{88 	
return99 
outCome99 
.99 
Reason99  
;99  !
}:: 	
};; 
}<< ˘
T/home/rb/source/rossbuggins-public/commcheck-api/Api/CommsCheckQuestionRequestDto.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct (
CommsCheckQuestionRequestDto :
(: ;
DateOnly 
DateOfBirth 
, 
DateOnly 
DateOfSmsUpdate 
, 
RfREnum 
? 
RfR 
) 
{		 
} 
U/home/rb/source/rossbuggins-public/commcheck-api/Api/CommsCheckQuestionResponseDto.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct )
CommsCheckQuestionResponseDto ;
(; <
string< B
ResultIdC K
)K L
{ 
} Å
?/home/rb/source/rossbuggins-public/commcheck-api/Api/RfREnum.cs
	namespace 	

CommsCheck
 
; 
public 
enum 
RfREnum 
{ 
DEA 
, 
CGA 
} ∞
N/home/rb/source/rossbuggins-public/commcheck-api/Commands/CheckCommsCommand.cs
	namespace 	

CommsCheck
 
; 
public 
class 
CheckCommsCommand 
( (
CommsCheckQuestionRequestDto ;
dto< ?
)? @
:A B
IRequestC K
<K L)
CommsCheckQuestionResponseDtoL i
>i j
{ 
public 
(
CommsCheckQuestionRequestDto '
Dto( +
=>, .
dto/ 2
;2 3
} Ó
U/home/rb/source/rossbuggins-public/commcheck-api/Commands/CheckCommsCommandHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class $
CheckCommsCommandHandler %
(% &

ObjectPool 
< 
HashWrapper 
> 
shaPool #
,# $
ChannelWriter

 
<

  
CommsCheckItemWithId

 &
>

& '
writer

( .
)

. /
:

0 1
IRequestHandler 
< 
CheckCommsCommand %
,% &)
CommsCheckQuestionResponseDto' D
>D E
{ 
private 
static 
readonly 
Meter !
MyMeter" )
=* +
new, /
(/ 0
$str0 Z
,Z [
$str\ a
)a b
;b c
private 
static 
readonly 
Counter #
<# $
long$ (
>( )
HandledCounter* 8
=9 :
MyMeter; B
.B C
CreateCounterC P
<P Q
longQ U
>U V
(V W
$strW 
)	 Ä
;
Ä Å
public 

async 
Task 
< )
CommsCheckQuestionResponseDto 3
>3 4
Handle5 ;
(; <
CheckCommsCommand 
request !
,! "
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
item 
= 
CommsCheckItem !
.! "
FromDto" )
() *
request* 1
.1 2
Dto2 5
)5 6
;6 7
var 
wrapper 
= 
shaPool 
. 
Get !
(! "
)" #
;# $
var 
	pooledSha 
= 
await 
wrapper %
.% &
GetSha& ,
(, -
item- 1
,1 2
$str3 U
)U V
;V W
shaPool 
. 
Return 
( 
wrapper 
) 
;  
var 

itemWithId 
= 
new  
CommsCheckItemWithId 1
(1 2
	pooledSha2 ;
,; <
item= A
)A B
;B C
await 
writer 
. 

WriteAsync 
(  

itemWithId  *
)* +
;+ ,
HandledCounter 
. 
Add 
( 
$num 
) 
; 
return   
new   )
CommsCheckQuestionResponseDto   0
(  0 1
	pooledSha  1 :
)  : ;
;  ; <
}!! 
}"" ∫
T/home/rb/source/rossbuggins-public/commcheck-api/Commands/CheckCommsDirectCommand.cs
	namespace 	

CommsCheck
 
; 
public 
class #
CheckCommsDirectCommand $
($ %(
CommsCheckQuestionRequestDto% A
dtoB E
)E F
:G H
IRequestI Q
<Q R'
CommsCheckAnswerResponseDtoR m
>m n
{ 
public 
(
CommsCheckQuestionRequestDto '
Dto( +
=>, .
dto/ 2
;2 3
} ¬!
[/home/rb/source/rossbuggins-public/commcheck-api/Commands/CheckCommsDirectCommandHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class *
CheckCommsDirectCommandHandler +
(+ ,

ObjectPool 
< 
HashWrapper 
> 
shaPool #
,# $

ICommCheck		 
check		 
)		 
:		 
IRequestHandler

 
<

 #
CheckCommsDirectCommand

 +
,

+ ,'
CommsCheckAnswerResponseDto

- H
>

H I
{ 
private 
static 
readonly 
Meter !
MyMeter" )
=* +
new, /
(/ 0
$str0 `
,` a
$strb g
)g h
;h i
private 
static 
readonly 
Counter #
<# $
long$ (
>( )
HandledCounter* 8
=9 :
MyMeter; B
.B C
CreateCounterC P
<P Q
longQ U
>U V
(V W
$str	W Ö
)
Ö Ü
;
Ü á
private 
static 
readonly 
	Histogram %
<% &
double& ,
>, -
ProcessTime. 9
=: ;
MyMeter< C
.C D
CreateHistogramD S
<S T
doubleT Z
>Z [
([ \
$str	\ ç
)
ç é
;
é è
private 
static 
readonly 
UpDownCounter )
<) *
long* .
>. /
CurrentlyProcessing0 C
=D E
MyMeterE L
.L M
CreateUpDownCounterM `
<` a
longa e
>e f
(f g
$str	g î
)
î ï
;
ï ñ
public 

async 
Task 
< '
CommsCheckAnswerResponseDto 1
>1 2
Handle3 9
(9 :#
CheckCommsDirectCommand: Q
requestR Y
,Y Z
CancellationToken[ l
cancellationTokenm ~
)~ 
{ 
throw 
new #
NotImplementedException )
() *
)* +
;+ ,
var 
sw 
= 
	Stopwatch 
. 
StartNew #
(# $
)$ %
;% &
CurrentlyProcessing 
. 
Add 
(  
$num  !
)! "
;" #
var 
item 
= 
CommsCheckItem !
.! "
FromDto" )
() *
request* 1
.1 2
Dto2 5
)5 6
;6 7
var 
wrapper 
= 
shaPool 
. 
Get !
(! "
)" #
;# $
var 
	pooledSha 
= 
await 
wrapper %
.% &
GetSha& ,
(, -
item- 1
,1 2
$str3 U
)U V
;V W
shaPool 
. 
Return 
( 
wrapper 
) 
;  
var 

itemWithId 
= 
new  
CommsCheckItemWithId 1
(1 2
	pooledSha2 ;
,; <
item= A
)A B
;B C
await 
check 
. 
Check 
( 

itemWithId $
)$ %
;% &
sw 

.
 
Stop 
( 
) 
; 
HandledCounter   
.   
Add   
(   
$num   
)   
;   
ProcessTime!! 
.!! 
Record!! 
(!! 
sw!! 
.!! 
Elapsed!! %
.!!% &
TotalSeconds!!& 2
)!!2 3
;!!3 4
CurrentlyProcessing"" 
."" 
Add"" 
(""  
-""  !
$num""! "
)""" #
;""# $
}$$ 
}%% è
Q/home/rb/source/rossbuggins-public/commcheck-api/Commands/CommsCheckItemWithId.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct  
CommsCheckItemWithId 2
(3 4
string4 :
Id; =
,= >
CommsCheckItem? M
ItemN R
)R S
;S Tå
O/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckAnswer.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct 
CommsCheckAnswer .
(. /
string 

ResultId 
, 
string 

RequestString 
, 
params 

IRuleOutcome 
[ 
] 
Outcomes "
)" #
{ 
} „
M/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckItem.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct 
CommsCheckItem ,
(, -
DateOnly 
DateOfBirth 
, 
DateOnly 
DateOfSmsUpdated 
, 
IReasonForRemoval 
ReasonForRemoval &
)& '
{ 
public 

int 
DaysOld 
=> 
DateOnly 
. 
FromDateTime 
( 
DateTime &
.& '
Now' *
)* +
.+ ,
	DayNumber, 5
-6 7
DateOfBirth8 C
.C D
	DayNumberD M
;M N
public

 

int

 
DaySinceSmsUpdate

  
=>

! #
DateOnly 
. 
FromDateTime 
( 
DateTime &
.& '
Now' *
)* +
.+ ,
	DayNumber, 5
-6 7
DateOfSmsUpdated8 H
.H I
	DayNumberI R
;R S
public 

int 
YearsOld 
=> 
( 
int 
)  
Math  $
.$ %
Floor% *
(* +
DaysOld+ 2
/3 4
(5 6
float6 ;
); <
$num< ?
)? @
;@ A
public 

static 
CommsCheckItem  
FromDto! (
(( )(
CommsCheckQuestionRequestDto) E
dtoF I
)I J
{ 
return 
new 
CommsCheckItem !
(! "
dto 
. 
DateOfBirth 
, 
dto 
. 
DateOfSmsUpdate 
,  
IReasonForRemoval 
. 
FromEnum &
(& '
dto' *
.* +
RfR+ .
). /
)/ 0
;0 1
} 
} ´"
j/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/CommsCheckRulesEngine.cs
	namespace 	

CommsCheck
 
; 
public		 
class		 !
CommsCheckRulesEngine		 "
:		# $

ICommCheck		% /
{

 
private 
readonly 
RulesEngine  
_rulesEngine! -
;- .
private 
readonly 
ILogger 
< !
CommsCheckRulesEngine 2
>2 3
_logger4 ;
;; <
private 
readonly 
IOptions 
< (
CommsCheckRulesEngineOptions :
>: ;
_options< D
;D E
private 
readonly 
IEnumerable  
<  !)
ICommsCheckRulesEngineRuleRun! >
<> ?
IContactType? K
>K L
>L M
_rulesN T
;T U
private 
readonly 

IPublisher 

_publisher  *
;* +
public 
!
CommsCheckRulesEngine  
(  !
ILogger	 
< !
CommsCheckRulesEngine &
>& '
logger( .
,. /
IOptions	 
< (
CommsCheckRulesEngineOptions .
>. /
options0 7
,7 8
IEnumerable	 
< )
ICommsCheckRulesEngineRuleRun 2
<2 3
IContactType3 ?
>? @
>@ A
rulesB G
,G H

IPublisher	 
	publisher 
) 
{ 
_logger 
= 
logger 
; 
_options 
= 
options 
; 
_rules 
= 
rules 
; 

_publisher 
= 
	publisher 
; 
var 
fileData 
= 
File 
. 
ReadAllText '
(' (
_options( 0
.0 1
Value1 6
.6 7
JsonPath7 ?
)? @
;@ A
_rulesEngine 
= 
LoadRulesEngine &
(& '
fileData' /
)/ 0
;0 1
} 
private 
RulesEngine 
LoadRulesEngine '
(' (
string( .
fileData/ 7
)7 8
{ 
var   
workflow   
=   
System   
.   
Text   "
.  " #
Json  # '
.  ' (
JsonSerializer  ( 6
.  6 7
Deserialize  7 B
<  B C
List  C G
<  G H
Workflow  H P
>  P Q
>  Q R
(  R S
fileData  S [
)  [ \
;  \ ]
if!! 

(!! 
workflow!! 
==!! 
null!! 
)!! 
throw"" 
new"" 
	Exception"" 
(""  
$"""  "
$str""" F
{""F G
_options""G O
.""O P
Value""P U
.""U V
JsonPath""V ^
}""^ _
"""_ `
)""` a
;""a b
var## 
rulesEngine## 
=## 
new## 
RulesEngine## )
(##) *
workflow##* 2
.##2 3
ToArray##3 :
(##: ;
)##; <
)##< =
;##= >
return$$ 
rulesEngine$$ 
;$$ 
}%% 
public'' 

async'' 
Task'' 
Check'' 
(''  
CommsCheckItemWithId'' 0
toCheck''1 8
)''8 9
{(( 
await)) 
Parallel)) 
.)) 
ForEachAsync)) #
())# $
_rules))$ *
,))* +
async)), 1
())2 3
rule))3 7
,))7 8
ct))9 ;
))); <
=>))= ?
{** 	
await++ 

_publisher++ 
.++ 
Publish++ $
(++$ %
new++% (
RunRuleEvent++) 5
(++5 6
rule++6 :
,++: ;
_rulesEngine++< H
,++H I
toCheck++J Q
)++Q R
)++R S
;++S T
},, 	
),,	 

;,,
 
}-- 
}.. à
q/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/CommsCheckRulesEngineOptions.cs
	namespace 	

CommsCheck
 
; 
public 
class (
CommsCheckRulesEngineOptions )
{ 
public 

string 
JsonPath 
{ 
get 
; 
set "
;" #
}# $
=% &
string' -
.- .
Empty. 3
;3 4
} ˘
w/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/CommsCheckRulesEngineRuleRunEvents.cs
	namespace 	

CommsCheck
 
; 
public 
class .
"CommsCheckRulesEngineRuleRunEvents /
</ 0
T0 1
>1 2
(2 3
ILogger 
< .
"CommsCheckRulesEngineRuleRunEvents .
<. /
T/ 0
>0 1
>1 2
_logger3 :
,: ;

IPublisher		 

_publisher		 
)		 
:		 )
ICommsCheckRulesEngineRuleRun

 !
<

! "
T

" #
>

# $
where

% *
T

+ ,
:

- .
IContactType

/ ;
{ 
public 

async 
Task 
Run 
( 
RulesEngine %
.% &
RulesEngine& 1
rulesEngine2 =
,= > 
CommsCheckItemWithId? S
toCheckT [
)[ \
{ 
var 
currentMethod 
= 
	GetMethod %
(% &
)& '
;' (
var 
	ruleRunId 
= 
Guid 
. 
NewGuid $
($ %
)% &
;& '
await 

_publisher 
. 
Publish  
(  !
new 
RulesLoadedEvent  
(  !
	ruleRunId! *
,* +
rulesEngine, 7
,7 8
currentMethod9 F
,F G
toCheckH O
)O P
)P Q
;Q R
} 
private 
string 
	GetMethod 
( 
) 
=> !
typeof" (
(( )
T) *
)* +
.+ ,
Name, 0
;0 1
} µ
l/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Events/ICommsCheckEvent.cs
	namespace 	

CommsCheck
 
; 
public 
	interface 
ICommsCheckEvent !
:" #
INotification$ 1
{ 
} ˇ
l/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Events/ItemToCheckEvent.cs
	namespace 	

CommsCheck
 
; 
public 
class 
ItemToCheckEvent 
(  
CommsCheckItemWithId 3
item4 8
)8 9
:9 :
ICommsCheckEvent; K
{ 
public 
 
CommsCheckItemWithId 
Item  $
=>% '
item( ,
;, -
} â
q/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Events/MaybeItemToCheckEvent.cs
	namespace 	

CommsCheck
 
; 
public 
class !
MaybeItemToCheckEvent "
(" # 
CommsCheckItemWithId# 7
item8 <
)< =
:> ?
ICommsCheckEvent@ P
{ 
public 
 
CommsCheckItemWithId 
Item  $
=>% '
item( ,
;, -
} è	
t/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Events/RuleResultsCombinedEvent.cs
	namespace 	

CommsCheck
 
; 
public 
class $
RuleResultsCombinedEvent %
(% &
Guid 
	ruleRunId	 
, 
string 

method 
,  
CommsCheckItemWithId 
toCheck  
,  !
IEnumerable		 
<		 
IRuleOutcome		 
>		 
outcomes		 &
)		& '
:		( )
ICommsCheckEvent		* :
{

 
public 

Guid 
	RuleRubId 
=> 
	ruleRunId &
;& '
public 

IEnumerable 
< 
IRuleOutcome #
># $
Outcomes% -
=>. 0
outcomes1 9
;9 :
public 
 
CommsCheckItemWithId 
ToCheck  '
=>( *
toCheck+ 2
;2 3
public 

string 
Method 
=> 
method "
;" #
} û
t/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Events/RuleRunMethodResultEvent.cs
	namespace 	

CommsCheck
 
; 
public 
class $
RuleRunMethodResultEvent %
(% &
Guid 
	ruleRunId	 
, 
string 

method 
,  
CommsCheckItemWithId 
toCheck  
,  !
IRuleOutcome		 
outcome		 
)		 
:		 
ICommsCheckEvent		 ,
{

 
public 

Guid 
	RuleRubId 
=> 
	ruleRunId &
;& '
public 

IRuleOutcome 
Outcome 
=>  "
outcome# *
;* +
public 
 
CommsCheckItemWithId 
ToCheck  '
=>( *
toCheck+ 2
;2 3
public 

string 
Method 
=> 
method "
;" #
} Ë
l/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Events/RulesLoadedEvent.cs
	namespace 	

CommsCheck
 
; 
public 
class 
RulesLoadedEvent 
( 
Guid 
	ruleRunId	 
, 
RulesEngine 
. 
RulesEngine 
rulesEngine '
,' (
string 

method 
,  
CommsCheckItemWithId 
toCheck  
)  !
:" #
ICommsCheckEvent$ 4
{		 
public

 

Guid

 
	RuleRunId

 
=>

 
	ruleRunId

 &
;

& '
public 

RulesEngine 
. 
RulesEngine "
RulesEngine# .
=>/ 1
rulesEngine2 =
;= >
public 

string 
Method 
=> 
method "
;" #
public 
 
CommsCheckItemWithId 
ToCheck  '
=>( *
toCheck+ 2
;2 3
} ñ
q/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Events/RulesRunCompleteEvent.cs
	namespace 	

CommsCheck
 
; 
public 
class !
RulesRunCompleteEvent "
(" #
Guid 
	ruleRunId	 
, 
IRuleOutcome 
outcome 
, 
string 

method 
,  
CommsCheckItemWithId 
toCheck  
)		 
:		 
ICommsCheckEvent		 
{

 
public 

Guid 
	RuleRunId 
=> 
	ruleRunId &
;& '
public 

IRuleOutcome 
Outcome 
=>  "
outcome# *
;* +
public 

string 
Method 
=> 
method "
;" #
public 
 
CommsCheckItemWithId 
ToCheck  '
=>( *
toCheck+ 2
;2 3
} ª
h/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Events/RunRuleEvent.cs
	namespace 	

CommsCheck
 
; 
public 
class 
RunRuleEvent 
( )
ICommsCheckRulesEngineRuleRun !
<! "
IContactType" .
>. /
rule0 4
,4 5
RulesEngine 
. 
RulesEngine 
rules !
,! " 
CommsCheckItemWithId 
toCheck  
)  !
:		 
ICommsCheckEvent		 
{

 
public 
)
ICommsCheckRulesEngineRuleRun (
<( )
IContactType) 5
>5 6
Rule7 ;
=>< >
rule? C
;C D
public 

RulesEngine 
. 
RulesEngine "
Rules# (
=>) +
rules, 1
;1 2
public 
 
CommsCheckItemWithId 
ToCheck  '
=>( *
toCheck+ 2
;2 3
} ç
u/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/ItemToCheckEventHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class #
ItemToCheckEventHandler $
($ %

ICommCheck% /
_check0 6
)6 7
:8 9 
INotificationHandler: N
<N O
ItemToCheckEventO _
>_ `
{ 
public 

async 
Task 
Handle 
( 
ItemToCheckEvent -
notification. :
,: ;
CancellationToken< M
cancellationTokenN _
)_ `
{		 
await

 
_check

 
.

 
Check

 
(

 
notification

 '
.

' (
Item

( ,
)

, -
;

- .
} 
} ≥0
z/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/MaybeItemToCheckEventHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class (
MaybeItemToCheckEventHandler )
() *

IPublisher		 

_publisher		 
,		 
ILogger

 
<

 (
MaybeItemToCheckEventHandler

 (
>

( )
_logger

* 1
,

1 2
IDistributedCache 
_cache 
) 
:  
INotificationHandler 
< !
MaybeItemToCheckEvent .
>. /
{ 
private 
static 
readonly 
Meter !
MyMeter" )
=* +
new, /
(/ 0
$str0 Y
,Y Z
$str[ `
)` a
;a b
private 
static 
readonly 
Counter #
<# $
long$ (
>( )
ProcessCheckCount* ;
=< =
MyMeter> E
.E F
CreateCounterF S
<S T
longT X
>X Y
(Y Z
$strZ n
)n o
;o p
private 
static 
readonly 
	Histogram %
<% &
double& ,
>, -
ProcessTime. 9
=: ;
MyMeter< C
.C D
CreateHistogramD S
<S T
doubleT Z
>Z [
([ \
$str\ {
){ |
;| }
private 
static 
readonly 
UpDownCounter )
<) *
long* .
>. /
CurrentlyProcessing0 C
=D E
MyMeterF M
.M N
CreateUpDownCounterN a
<a b
longb f
>f g
(g h
$str	h É
)
É Ñ
;
Ñ Ö
public 

async 
Task 
Handle 
( !
MaybeItemToCheckEvent 2
notification3 ?
,? @
CancellationTokenA R
cancellationTokenS d
)d e
{ 
await #
TryProcessCommCheckItem %
(% &
notification& 2
.2 3
Item3 7
)7 8
;8 9
} 
private 
async 
Task #
TryProcessCommCheckItem .
(. / 
CommsCheckItemWithId/ C
itemD H
)H I
{ 
var 
sw 
= 
MetricStart 
( 
) 
; 
try 
{ 	
await  
ProcessCommCheckItem &
(& '
item' +
)+ ,
;, -
} 	
catch 
( 
	Exception 
e 
) 
{   	
ProcessException!! 
(!! 
e!! 
)!! 
;!!  
}"" 	
finally## 
{$$ 	
MetricsStop%% 
(%% 
sw%% 
)%% 
;%% 
}&& 	
}'' 
private)) 
void)) 
ProcessException)) !
())! "
	Exception))" +
ex)), .
))). /
{** 
_logger++ 
.++ 
LogError++ 
(++ 
ex++ 
,++ 
$str++ =
)++= >
;++> ?
},, 
private.. 
	Stopwatch.. 
MetricStart.. !
(..! "
).." #
{// 
var00 
sw00 
=00 
	Stopwatch00 
.00 
StartNew00 #
(00# $
)00$ %
;00% &
CurrentlyProcessing11 
.11 
Add11 
(11  
$num11  !
)11! "
;11" #
return22 
sw22 
;22 
}33 
private55 
void55 
MetricsStop55 
(55 
	Stopwatch55 &
sw55' )
)55) *
{66 
sw77 

.77
 
Stop77 
(77 
)77 
;77 
ProcessTime88 
.88 
Record88 
(88 
sw88 
.88 
Elapsed88 %
.88% &
TotalSeconds88& 2
)882 3
;883 4
CurrentlyProcessing99 
.99 
Add99 
(99  
-99  !
$num99! "
)99" #
;99# $
}:: 
private<< 
async<< 
Task<<  
ProcessCommCheckItem<< +
(<<+ , 
CommsCheckItemWithId<<, @
item<<A E
)<<E F
{== 
if>> 

(>> 
await>> 
IsNotInCache>> 
(>> 
item>> #
.>># $
Id>>$ &
)>>& '
)>>' (
{?? 	
await@@ 
ProcessCheck@@ 
(@@ 
item@@ #
)@@# $
;@@$ %
}AA 	
}BB 
privateDD 
asyncDD 
TaskDD 
<DD 
boolDD 
>DD 
IsNotInCacheDD )
(DD) *
stringDD* 0
idDD1 3
)DD3 4
{EE 
varFF 

cacheEntryFF 
=FF 
awaitFF 
_cacheFF %
.FF% &
GetAsyncFF& .
(FF. /
idFF/ 1
)FF1 2
;FF2 3
returnGG 

cacheEntryGG 
==GG 
nullGG !
;GG! "
}HH 
privateJJ 
asyncJJ 
TaskJJ 
ProcessCheckJJ #
(JJ# $ 
CommsCheckItemWithIdJJ$ 8
itemJJ9 =
)JJ= >
{KK 
awaitLL 

_publisherLL 
.LL 
PublishLL  
(LL  !
newLL! $
ItemToCheckEventLL% 5
(LL5 6
itemLL6 :
)LL: ;
)LL; <
;LL< =
ProcessCheckCountMM 
.MM 
AddMM 
(MM 
$numMM 
)MM  
;MM  !
}NN 
}OO ⁄
}/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/RuleResultsCombinedEventHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class +
RuleResultsCombinedEventHandler ,
(, -

IPublisher- 7

_publisher8 B
)B C
:D E 
INotificationHandlerF Z
<Z [$
RuleResultsCombinedEvent[ s
>s t
{ 
public 

async 
Task 
Handle 
( $
RuleResultsCombinedEvent 5
notification6 B
,B C
CancellationTokenD U
cancellationTokenV g
)g h
{ 
var		 
blocked		 
=		 
notification		 "
.		" #
Outcomes		# +
.		+ ,
Where		, 1
(		1 2
x		2 3
=>		4 6
x		7 8
is		9 ;
RuleBlocked		< G
)		G H
.		H I
ToList		I O
(		O P
)		P Q
;		Q R
var

 
allowed

 
=

 
notification

 "
.

" #
Outcomes

# +
.

+ ,
Where

, 1
(

1 2
x

2 3
=>

4 6
x

7 8
is

9 ;
RuleAllowed

< G
)

G H
.

H I
ToList

I O
(

O P
)

P Q
;

Q R
var 
t 
= 
( 
blocked 
. 
Count 
>  
$num! "
," #
allowed$ +
.+ ,
Count, 1
>2 3
$num4 5
)5 6
switch7 =
{ 	
( 
true 
, 
_ 
) 
=> 

_publisher #
.# $
Publish$ +
(+ ,
new, /$
RuleRunMethodResultEvent0 H
(H I
notification 
. 
	RuleRubId &
,& '
notification 
. 
Method #
,# $
notification 
. 
ToCheck $
,$ %
blocked 
. 
First 
( 
) 
)  
)  !
,! "
( 
false 
, 
true 
) 
=> 

_publisher '
.' (
Publish( /
(/ 0
new0 3$
RuleRunMethodResultEvent4 L
(L M
notification 
. 
	RuleRubId &
,& '
notification 
. 
Method #
,# $
notification 
. 
ToCheck $
,$ %
allowed 
. 
First 
( 
) 
)  
)  !
,! "
_ 
=> 

_publisher 
. 
Publish #
(# $
new$ '$
RuleRunMethodResultEvent( @
(@ A
notification 
. 
	RuleRubId &
,& '
notification 
. 
Method #
,# $
notification 
. 
ToCheck $
,$ %
IRuleOutcome 
. 
Blocked $
($ %
notification% 1
.1 2
Method2 8
,8 9
$str: I
)I J
)J K
)K L
} 	
;	 

await!! 
t!! 
;!! 
}"" 
}## ‚
}/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/RuleRunMethodResultEventHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class +
RuleRunMethodResultEventHandler ,
(, -+
RuleRunMethodResultCacheService- L
_cacheM S
)S T
:  
INotificationHandler 
< $
RuleRunMethodResultEvent /
>/ 0
{ 
public 

async 
Task 
Handle 
( $
RuleRunMethodResultEvent 5
notification6 B
,B C
CancellationTokenD U
cancellationTokenV g
)g h
{		 
await

 
_cache

 
.

 &
NewOrUpdateCacheThreadSafe

 /
(

/ 0
notification

0 <
,

< =
cancellationToken

> O
)

O P
;

P Q
} 
} ê
z/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/RulesRunCompleteEventHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class (
RulesRunCompleteEventHandler )
() * 
RulesCombinerService* >
	_combiner? H
)H I
:  
INotificationHandler 
< !
RulesRunCompleteEvent ,
>, -
{ 
public 

async 
Task 
Handle 
( !
RulesRunCompleteEvent 2
notification3 ?
,? @
CancellationTokenA R
cancellationTokenS d
)d e
{ 
await		 
	_combiner		 
.		 
Combine		 
(		  
notification		  ,
)		, -
;		- .
}

 
} ¶
q/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/RunRuleEventHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class 
RunRuleEventHandler  
:! " 
INotificationHandler# 7
<7 8
RunRuleEvent8 D
>D E
{ 
public 

async 
Task 
Handle 
( 
RunRuleEvent )
notification* 6
,6 7
CancellationToken8 I
cancellationTokenJ [
)[ \
{ 
await		 
notification		 
.		 
Rule		 
.		  
Run		  #
(		# $
notification		$ 0
.		0 1
Rules		1 6
,		6 7
notification		8 D
.		D E
ToCheck		E L
)		L M
;		M N
}

 
} ∏
s/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/RunRulesAllowdHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class !
RunRulesAllowdHandler "
(" #

IPublisher# -

_publisher. 8
)8 9
:: ; 
INotificationHandler< P
<P Q
RulesLoadedEventQ a
>a b
{ 
public 

async 
Task 
Handle 
( 
RulesLoadedEvent -
request. 5
,5 6
CancellationToken7 H
cancellationTokenI Z
)Z [
{ 
var 
result 
= 
await 
RunRuleFunctions +
.+ ,

RunAllowed, 6
(6 7
request		 
.		 
Method		 
,		 
request

 
.

 
RulesEngine

 
,

  
request 
. 
ToCheck 
. 
Item  
)  !
;! "
await 

_publisher 
. 
Publish  
(  !
new! $!
RulesRunCompleteEvent% :
(: ;
request 
. 
	RuleRunId 
, 
result 
, 
request 
. 
Method 
, 
request 
. 
ToCheck 
) 
) 
; 
} 
} ‘
}/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/RunRulesExplicitBlockAllHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class +
RunRulesExplicitBlockAllHandler ,
(, -

IPublisher- 7

_publisher8 B
)B C
:D E 
INotificationHandlerF Z
<Z [
RulesLoadedEvent[ k
>k l
{ 
public 

async 
Task 
Handle 
( 
RulesLoadedEvent -
request. 5
,5 6
CancellationToken7 H
cancellationTokenI Z
)Z [
{ 
var		 
result		 
=		 
await		 
RunRuleFunctions		 +
.		+ ,
RunExplictBlockAll		, >
(		> ?
request

 
.

 
Method

 
,

 
request 
. 
RulesEngine 
,  
request 
. 
ToCheck 
. 
Item  
)  !
;! "
await 

_publisher 
. 
Publish  
(  !
new! $!
RulesRunCompleteEvent% :
(: ;
request 
. 
	RuleRunId 
, 
result 
, 
request 
. 
Method 
, 
request 
. 
ToCheck 
) 
) 
; 
} 
} À
z/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Handlers/RunRulesExplicitBlockHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class (
RunRulesExplicitBlockHandler )
() *

IPublisher* 4

_publisher5 ?
)? @
:A B 
INotificationHandlerC W
<W X
RulesLoadedEventX h
>h i
{ 
public 

async 
Task 
Handle 
( 
RulesLoadedEvent -
request. 5
,5 6
CancellationToken7 H
cancellationTokenI Z
)Z [
{ 
var		 
result		 
=		 
await		 
RunRuleFunctions		 +
.		+ ,
RunExplictBlock		, ;
(		; <
request

 
.

 
Method

 
,

 
request 
. 
RulesEngine 
,  
request 
. 
ToCheck 
. 
Item  
)  !
;! "
await 

_publisher 
. 
Publish  
(  !
new! $!
RulesRunCompleteEvent% :
(: ;
request 
. 
	RuleRunId 
, 
result 
, 
request 
. 
Method 
, 
request 
. 
ToCheck 
) 
) 
; 
} 
} ó
r/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/ICommsCheckRulesEngineRuleRun.cs
	namespace 	

CommsCheck
 
; 
public 
	interface )
ICommsCheckRulesEngineRuleRun .
<. /
out/ 2
T3 4
>4 5
where6 ;
T< =
:> ?
IContactType@ L
{ 
Task 
Run	 
( 
RulesEngine 
. 
RulesEngine $
rulesEngine% 0
,0 1 
CommsCheckItemWithId2 F
toCheckG N
)N O
;O P
} Ç7
}/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Services/RuleRunMethodResultCacheService.cs
	namespace 	

CommsCheck
 
; 
public

 
class

 +
RuleRunMethodResultCacheService

 ,
(

, -
IDistributedCache

- >
_cache

? E
)

E F
{ 
private 
SemaphoreSlim 
_slim 
=  !
new" %
SemaphoreSlim& 3
(3 4
$num4 5
,5 6
$num7 8
)8 9
;9 :
private 
Func 
< 
string 
, 
Task 
< 
byte "
[" #
]# $
?$ %
>% &
>& '
_getFromCache( 5
=6 7
async8 =
x> ?
=>@ B
awaitC H
_cacheI O
.O P
GetAsyncP X
(X Y
xY Z
)Z [
;[ \
public 

async 
Task &
NewOrUpdateCacheThreadSafe 0
(0 1$
RuleRunMethodResultEvent1 I
notificationJ V
,V W
CancellationTokenX i
cancellationTokenj {
){ |
{ 
await 
_slim 
. 
	WaitAsync 
( 
cancellationToken /
)/ 0
;0 1
try 
{ 	
var 
maybe 
= 
await %
GetMaybeIfCacheItemExists 7
(7 8
notification8 D
.D E
ToCheckE L
.L M
IdM O
)O P
;P Q
await  
NewOrUpdateCacheItem &
(& '
maybe' ,
,, -
notification. :
): ;
;; <
} 	
finally 
{ 	
_slim 
. 
Release 
( 
) 
; 
} 	
} 
private   
Task    
NewOrUpdateCacheItem   %
(  % &
Maybe  & +
<  + ,
byte  , 0
[  0 1
]  1 2
?  2 3
>  3 4
maybe  5 :
,  : ;$
RuleRunMethodResultEvent  < T
notification  U a
)  a b
{!! 
Func## 
<## 
byte## 
[## 
]## 
?## 
,## 
Task## 
>## 
newItem## #
=##$ %
x##& '
=>##( *
WriteNewItem##+ 7
(##7 8
notification##8 D
)##D E
;##E F
Func$$ 
<$$ 
byte$$ 
[$$ 
]$$ 
?$$ 
,$$ 
Task$$ 
>$$ 

updateItem$$ &
=$$' (
x$$) *
=>$$+ -
WriteUpdatedItem$$. >
($$> ?
x$$? @
,$$@ A
notification$$B N
)$$N O
;$$O P
var&& 
rVal&& 
=&& 
maybe&& 
.&& 
Fork&& 
(&& 
empty'' 
=>'' 
newItem'' 
('' 
empty'' "
)''" #
,''# $
full(( 
=>(( 

updateItem(( 
((( 
full(( #
)((# $
)(($ %
;((% &
return** 
rVal** 
;** 
}++ 
private-- 
async-- 
Task-- 
<-- 
Maybe-- 
<-- 
byte-- !
[--! "
]--" #
?--# $
>--$ %
>--% &%
GetMaybeIfCacheItemExists--' @
(--@ A
string--A G
id--H J
)--J K
=>--L N
await.. 
id// 
.// 

ToIdentity// 
(// 
)// 
.00 

MaybeAsync00 
(00 
async00 
(00 
_id00 "
)00" #
=>00$ &
await00' ,
_getFromCache00- :
(00: ;
_id00; >
)00> ?
)00? @
;00@ A
private33 
async33 
Task33 
WriteNewItem33 #
(33# $$
RuleRunMethodResultEvent33$ <
notification33= I
)33I J
{44 
var55 
	newAnswer55 
=55 
new55 
CommsCheckAnswer55 ,
(55, -
notification66 
.66 
ToCheck66  
.66  !
Id66! #
,66# $
notification77 
.77 
ToCheck77  
.77  !
ToString77! )
(77) *
)77* +
,77+ ,
notification88 
.88 
Outcome88  
)88  !
;88! "
var:: 
b:: 
=:: 
System:: 
.:: 
Text:: 
.:: 
Json::  
.::  !
JsonSerializer::! /
.::/ 0 
SerializeToUtf8Bytes::0 D
(::D E
	newAnswer::E N
)::N O
;::O P
await;; 
_cache;; 
.;; 
SetAsync;; 
(;; 
notification;; *
.;;* +
ToCheck;;+ 2
.;;2 3
Id;;3 5
,;;5 6
b;;7 8
);;8 9
;;;9 :
}<< 
private>> 
async>> 
Task>> 
WriteUpdatedItem>> '
(>>' (
byte>>( ,
[>>, -
]>>- .
existingBytes>>/ <
,>>< =$
RuleRunMethodResultEvent>>> V
notification>>W c
)>>c d
{?? 
var@@ 
exitingItem@@ 
=@@ 
System@@  
.@@  !
Text@@! %
.@@% &
Json@@& *
.@@* +
JsonSerializer@@+ 9
.@@9 :
Deserialize@@: E
<@@E F
CommsCheckAnswer@@F V
>@@V W
(@@W X
existingBytes@@X e
)@@e f
;@@f g
varAA 
updatedItemAA 
=AA 
exitingItemAA %
withAA& *
{AA+ ,
OutcomesAA- 5
=AA6 7
exitingItemAA8 C
.AAC D
OutcomesAAD L
.AAL M
AppendAAM S
(AAS T
notificationAAT `
.AA` a
OutcomeAAa h
)AAh i
.AAi j
ToArrayAAj q
(AAq r
)AAr s
}AAt u
;AAu v
varBB 
bBB 
=BB 
SystemBB 
.BB 
TextBB 
.BB 
JsonBB  
.BB  !
JsonSerializerBB! /
.BB/ 0 
SerializeToUtf8BytesBB0 D
(BBD E
updatedItemBBE P
)BBP Q
;BBQ R
awaitCC 
_cacheCC 
.CC 
SetAsyncCC 
(CC 
notificationCC *
.CC* +
ToCheckCC+ 2
.CC2 3
IdCC3 5
,CC5 6
bCC7 8
)CC8 9
;CC9 :
}DD 
}EE ·
r/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Services/RulesCombinerService.cs
	namespace 	

CommsCheck
 
; 
public 
class  
RulesCombinerService !
(! "

IPublisher" ,

_publisher- 7
)7 8
{  
ConcurrentDictionary 
< 
Guid 
, 
List #
<# $
IRuleOutcome$ 0
>0 1
>1 2
	_outcomes3 <
== >
new? B 
ConcurrentDictionaryC W
<W X
GuidX \
,\ ]
List^ b
<b c
IRuleOutcomec o
>o p
>p q
(q r
)r s
;s t
public		 

Task		 
Combine		 
(		 !
RulesRunCompleteEvent		 -
notification		. :
)		: ;
{

 
var 
outcomes 
= 
	_outcomes  
.  !
AddOrUpdate! ,
(, -
notification 
. 
	RuleRunId "
," #
new 
List 
< 
IRuleOutcome !
>! "
{# $
notification% 1
.1 2
Outcome2 9
}: ;
,; <
( 
id 
, 
existing 
) 
=> 
new !
List" &
<& '
IRuleOutcome' 3
>3 4
{5 6
notification7 C
.C D
OutcomeD K
}L M
.M N
ConcatN T
(T U
existingU ]
)] ^
.^ _
ToList_ e
(e f
)f g
)g h
;h i
CheckIfAllThere 
( 
notification $
,$ %
outcomes& .
). /
;/ 0
return 
Task 
. 
CompletedTask !
;! "
} 
private 
Task 
CheckIfAllThere  
(  !!
RulesRunCompleteEvent! 6
notification7 C
,C D
ListE I
<I J
IRuleOutcomeJ V
>V W
outcomesX `
)` a
{ 
if 

( 
outcomes 
. 
Count 
== 
$num 
)  
{ 	

_publisher 
. 
Publish 
( 
new "$
RuleResultsCombinedEvent# ;
(; <
notification 
. 
	RuleRunId $
,$ %
notification 
. 
Method !
,! "
notification 
. 
ToCheck "
," #
outcomes 
) 
) 
; 
	_outcomes   
.   
Remove   
(   
notification   )
.  ) *
	RuleRunId  * 3
,  3 4
out  5 8
_  9 :
)  : ;
;  ; <
}!! 	
return"" 
Task"" 
."" 
CompletedTask"" !
;""! "
}## 
}$$ ·&
n/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/CommsCheckRulesEngine/Services/RunRuleFunctions.cs
	namespace 	

CommsCheck
 
; 
public 
static 
class 
RunRuleFunctions $
{ 
public 

static 
async 
Task 
< 
IRuleOutcome )
>) *
RunExplictBlockAll+ =
(= >
string		 
method		 
,		 
RulesEngine

 
.

 
RulesEngine

 
rulesEngine

  +
,

+ ,
CommsCheckItem 
toCheck 
) 
{ 
return 
await 
RunExplictBlock $
($ %
$str% *
,* +
method, 2
,2 3
rulesEngine4 ?
,? @
toCheckA H
)H I
;I J
} 
public 

static 
async 
Task 
< 
IRuleOutcome )
>) *
RunExplictBlock+ :
(: ;
string 
method 
, 
RulesEngine 
. 
RulesEngine 
rulesEngine  +
,+ ,
CommsCheckItem 
toCheck 
) 
{ 
return 
await 
RunExplictBlock $
($ %
method% +
,+ ,
method- 3
,3 4
rulesEngine5 @
,@ A
toCheckB I
)I J
;J K
} 
private 
static 
async 
Task 
< 
IRuleOutcome *
>* +
RunExplictBlock, ;
(; <
string 
methodToCheck 
, 
string 
methodToLog 
, 
RulesEngine 
. 
RulesEngine 
rulesEngine  +
,+ ,
CommsCheckItem 
toCheck 
) 
{ 
return 
await 
RunRules 
( 
$str 
, 
methodToCheck   
,   
rulesEngine!! 
,!! 
toCheck"" 
,"" 
(## 
str## 
)## 
=>## 
IRuleOutcome## !
.##! "
Blocked##" )
(##) *
methodToLog##* 5
,##5 6
str##7 :
)##: ;
)##; <
;##< =
}$$ 
public&& 

static&& 
async&& 
Task&& 
<&& 
IRuleOutcome&& )
>&&) *

RunAllowed&&+ 5
(&&5 6
string'' 
method'' 
,'' 
RulesEngine(( 
.(( 
RulesEngine(( 
rulesEngine((  +
,((+ ,
CommsCheckItem)) 
toCheck)) 
))) 
{** 
return++ 
await++ 
RunRules++ 
(++ 
$str,, 
,,, 
method-- 
,-- 
rulesEngine.. 
,.. 
toCheck// 
,// 
(00 
str00 
)00 
=>00 
IRuleOutcome00 !
.00! "
Allowed00" )
(00) *
method00* 0
,000 1
str002 5
)005 6
)006 7
;007 8
}11 
private33 
static33 
async33 
Task33 
<33 
IRuleOutcome33 *
>33* +
RunRules33, 4
(334 5
string44 

ruleSet44 
,44 
string55 

method55 
,55 
RulesEngine66 
.66 
RulesEngine66 
rulesEngine66 '
,66' (
CommsCheckItem77 
toCheck77 
,77 
Func88 
<88 	
string88	 
,88 
IRuleOutcome88 
>88 
	onSuccess88 (
)88( )
{99 
var:: 
results:: 
=:: 
await:: 
rulesEngine:: '
.::' ( 
ExecuteAllRulesAsync::( <
(::< =
ruleSet::= D
+::E F
$str::G J
+::K L
method::M S
,::S T
toCheck::U \
)::\ ]
;::] ^
IRuleOutcome== 
rVal== 
=== 
IRuleOutcome== (
.==( )
Ignored==) 0
(==0 1
)==1 2
;==2 3
results?? 
.?? 
	OnSuccess?? 
(?? 
(?? 
a?? 
)?? 
=>??  
{@@ 	
rValAA 
=AA 
	onSuccessAA 
(AA 
aAA 
)AA 
;AA  
}BB 	
)BB	 

;BB
 
returnDD 
rValDD 
;DD 
}EE 
}FF ˚
O/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ContactTypes/App.cs
	namespace 	

CommsCheck
 
; 
public 
class 
App 
: 
IContactType 
; ˇ
Q/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ContactTypes/Email.cs
	namespace 	

CommsCheck
 
; 
public 
class 
Email 
: 
IContactType 
;  Û
X/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ContactTypes/IContactType.cs
	namespace 	

CommsCheck
 
; 
public 
	interface 
IContactType 
{ 
} Å
R/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ContactTypes/Postal.cs
	namespace 	

CommsCheck
 
; 
public 
class 
Postal 
: 
IContactType  
;  !˚
O/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ContactTypes/Sms.cs
	namespace 	

CommsCheck
 
; 
public 
class 
Sms 
: 
IContactType 
; Î
I/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ICommCheck.cs
	namespace 	

CommsCheck
 
; 
public 
	interface 

ICommCheck 
{ 
Task 	
Check
 
(  
CommsCheckItemWithId $
toCheck% ,
), -
;- .
} Ú
M/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ICommCheckRule.cs
	namespace 	

CommsCheck
 
; 
public 
	interface 
ICommCheckRule 
{ 
public 

IRuleOutcome 
Block 
( 
string $
method% +
,+ ,
CommsCheckItem- ;
request< C
)C D
=>E G
IRuleOutcomeH T
.T U
IgnoredU \
(\ ]
)] ^
;^ _
} 
public 
	interface 
ICommCheckRule 
<  
T  !
>! "
:# $
ICommCheckRule% 3
where4 9
T: ;
:< =
IContactType> J
{		 
public

 
IRuleOutcome

 
Allowed

 #
(

# $
string

$ *
method

+ 1
,

1 2
CommsCheckItem

3 A
request

B I
)

I J
=>

K M
IRuleOutcome

N Z
.

Z [
Ignored

[ b
(

b c
)

c d
;

d e
} ™
V/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ReasonForRemovals/Death.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct 
Death #
:$ %
IReasonForRemoval& 7
{ 
public 

string 
Code 
=> 
$str 
;  
} È
b/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ReasonForRemovals/IReasonForRemoval.cs
	namespace 	

CommsCheck
 
; 
public 
	interface 
IReasonForRemoval "
{ 
string 

Code 
{ 
get 
; 
} 
public 

static 
Death 
Death 
=>  
new! $
Death% *
(* +
)+ ,
;, -
public 

static 
	MovedAway 
	MovedAway %
=>& (
new) ,
	MovedAway- 6
(6 7
)7 8
;8 9
public 

static 
NoReasonForRemoval $
None% )
=>* ,
new- 0
NoReasonForRemoval1 C
(C D
)D E
;E F
public		 

bool		 
IsEmpty		 
(		 
)		 
=>		 
this		 !
is		" $
NoReasonForRemoval		% 7
;		7 8
public 

bool 
NotSet 
=> 
this 
is !
NoReasonForRemoval" 4
;4 5
public 

bool 
HasCode 
=> 
! 
( 
this "
is# %
NoReasonForRemoval& 8
)8 9
;9 :
public 

static 
IReasonForRemoval #
FromEnum$ ,
(, -
RfREnum- 4
?4 5
e6 7
)7 8
=>9 ;
e 
switch 
{ 
null 
=> 
IReasonForRemoval !
.! "
None" &
,& '
RfREnum 
. 
DEA 
=> 
IReasonForRemoval (
.( )
Death) .
,. /
RfREnum 
. 
CGA 
=> 
IReasonForRemoval (
.( )
	MovedAway) 2
,2 3
_ 	
=>
 
throw 
new #
NotImplementedException .
(. /
)/ 0
} 
; 
} ≤
Z/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ReasonForRemovals/MovedAway.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct 
	MovedAway '
:( )
IReasonForRemoval* ;
{ 
public 

string 
Code 
=> 
$str 
;  
} æ
c/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/ReasonForRemovals/NoReasonForRemoval.cs
	namespace 	

CommsCheck
 
; 
public 
record 
NoReasonForRemoval  
:! "
IReasonForRemoval# 4
{ 
public 

string 
Code 
=> 
string  
.  !
Empty! &
;& '
} œ
X/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/RuleOutcomes/IRuleOutcome.cs
	namespace 	

CommsCheck
 
; 
[ 
JsonPolymorphic 
( &
UnknownDerivedTypeHandling 
=  *
JsonUnknownDerivedTypeHandling! ?
.? @%
FallBackToNearestAncestor@ Y
)Y Z
]Z [
[ 
JsonDerivedType 
( 
typeof 
( 
RuleAllowed #
)# $
,$ %
typeDiscriminator& 7
:7 8
$str9 F
)F G
]G H
[ 
JsonDerivedType 
( 
typeof 
( 
RuleBlocked #
)# $
,$ %
typeDiscriminator& 7
:7 8
$str9 F
)F G
]G H
[		 
JsonDerivedType		 
(		 
typeof		 
(		 
RuleIgnored		 #
)		# $
,		$ %
typeDiscriminator		& 7
:		7 8
$str		9 F
)		F G
]		G H
public

 
	interface

 
IRuleOutcome

 
:

 

IEquatable

  *
<

* +
IRuleOutcome

+ 7
>

7 8
{ 
string 

Method 
{ 
get 
; 
} 
string 

Reason 
{ 
get 
; 
} 
public 

static 
IRuleOutcome 
Allowed &
(& '
string' -
method. 4
,4 5
string6 <
reason= C
)C D
=>E G
newH K
RuleAllowedL W
(W X
methodX ^
,^ _
reason` f
)f g
;g h
public 

static 
IRuleOutcome 
Blocked &
(& '
string' -
method. 4
,4 5
string6 <
reason= C
)C D
=>E G
newH K
RuleBlockedL W
(W X
methodX ^
,^ _
reason` f
)f g
;g h
public 

static 
IRuleOutcome 
Ignored &
(& '
)' (
=>( *
new+ .
RuleIgnored/ :
(: ;
); <
;< =
public 

bool 
	IsAllowed 
( 
) 
{ 
return 
this 
is 
RuleAllowed "
;" #
} 
public 

bool 
	IsBlocked 
( 
) 
{ 
return 
this 
is 
RuleBlocked "
;" #
} 
} Û
W/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/RuleOutcomes/RuleAllowed.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct 
RuleAllowed )
() *
string* 0
Method1 7
,7 8
string9 ?
Reason@ F
)F G
:H I
IRuleOutcomeJ V
{ 
public 

bool 
Equals 
( 
IRuleOutcome #
?# $
other% *
)* +
{ 
if 

( 
other 
is 
RuleAllowed  
o! "
)" #
return 
o 
== 
this 
; 
return 
false 
; 
}		 
}

 Û
W/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/RuleOutcomes/RuleBlocked.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct 
RuleBlocked )
() *
string* 0
Method1 7
,7 8
string9 ?
Reason@ F
)F G
:G H
IRuleOutcomeI U
{ 
public 

bool 
Equals 
( 
IRuleOutcome #
?# $
other% *
)* +
{ 
if 

( 
other 
is 
RuleBlocked  
o! "
)" #
return 
o 
== 
this 
; 
return 
false 
; 
}		 
}

 Û
W/home/rb/source/rossbuggins-public/commcheck-api/CommsCheck/RuleOutcomes/RuleIgnored.cs
	namespace 	

CommsCheck
 
; 
public 
readonly 
record 
struct 
RuleIgnored )
() *
string* 0
Method1 7
,7 8
string9 ?
Reason@ F
)F G
:H I
IRuleOutcomeI U
{ 
public 

bool 
Equals 
( 
IRuleOutcome #
?# $
other% *
)* +
{ 
if 

( 
other 
is 
RuleIgnored  
o! "
)" #
return 
o 
== 
this 
; 
return 
false 
; 
}		 
}

 €
Y/home/rb/source/rossbuggins-public/commcheck-api/Extensions/CommsCheckExtensionMethods.cs
	namespace 	

CommsCheck
 
; 
public 
static 
class &
CommsCheckExtensionMethods .
{ 
public 

static 
IServiceCollection $
AddCommsCheck% 2
(2 3
this3 7
IServiceCollection8 J
servicesK S
,S T
ActionU [
<[ \
CommsCheckOptions\ m
>m n
optionso v
)v w
{		 
services

 
.

 
AddCommsCheck

 
(

 
)

  
;

  !
var 
optionsInstance 
= 
new !
CommsCheckOptions" 3
(3 4
services4 <
)< =
;= >
options 
( 
optionsInstance 
)  
;  !
return 
services 
; 
} 
public 

static 
IServiceCollection $
AddCommsCheck% 2
(2 3
this3 7
IServiceCollection8 J
servicesK S
)S T
{ 
services 
. 
AddHostedService !
<! "#
CommsCheckHostedService" 9
>9 :
(: ;
); <
;< =
services 
. 
AddSingleton 
( 
Channel %
.% &
CreateUnbounded& 5
<5 6 
CommsCheckItemWithId6 J
>J K
(K L
newL O#
UnboundedChannelOptionsP g
(g h
)h i
{j k
SingleReaderl x
=y z
true{ 
}
Ä Å
)
Å Ç
)
Ç É
;
É Ñ
services 
. 
AddSingleton 
( 
svc !
=>" $
svc% (
.( )
GetRequiredService) ;
<; <
Channel< C
<C D 
CommsCheckItemWithIdD X
>X Y
>Y Z
(Z [
)[ \
.\ ]
Reader] c
)c d
;d e
services 
. 
AddSingleton 
( 
svc !
=>" $
svc% (
.( )
GetRequiredService) ;
<; <
Channel< C
<C D 
CommsCheckItemWithIdD X
>X Y
>Y Z
(Z [
)[ \
.\ ]
Writer] c
)c d
;d e
services 
. 

AddMediatR 
( 
config "
=># %
{ 	
config 
. &
MediatorImplementationType -
=. /
typeof0 6
(6 7(
PublishWithMetricsAndLogging7 S
)S T
;T U
config 
. 2
&RegisterServicesFromAssemblyContaining 9
<9 :
CommsCheckOptions: K
>K L
(L M
)M N
;N O
config 
. 
AddOpenBehavior "
(" #
typeof# )
() *#
LoggingCommandsBehavior* A
<A B
,B C
>C D
)D E
)E F
;F G
} 	
)	 

;
 
return 
services 
; 
} 
}   ·
[/home/rb/source/rossbuggins-public/commcheck-api/Extensions/CommsCheckNativeRulesOptions.cs
	namespace 	

CommsCheck
 
; 
public 
class (
CommsCheckNativeRulesOptions )
() *
IServiceCollection* <
services= E
)E F
{ 
public 
(
CommsCheckNativeRulesOptions '
AddRule( /
</ 0
T0 1
>1 2
(2 3
)3 4
where5 :
T; <
:= >
class? D
,D E
ICommCheckRuleF T
{ 
services 
. 
AddTransient 
< 
ICommCheckRule ,
,, -
T. /
>/ 0
(0 1
)1 2
;2 3
return 
this 
; 
}		 
public 
(
CommsCheckNativeRulesOptions '
AddRule( /
</ 0
U0 1
,1 2
T3 4
>4 5
(5 6
)6 7
where 
T 
: 
class 
, 
ICommCheckRule '
<' (
U( )
>) *
where 
U 
: 
IContactType 
{ 
services 
. 
AddTransient 
< 
ICommCheckRule ,
<, -
U- .
>. /
,/ 0
T1 2
>2 3
(3 4
)4 5
;5 6
return 
this 
; 
} 
} Æ>
P/home/rb/source/rossbuggins-public/commcheck-api/Extensions/CommsCheckOptions.cs
	namespace 	

CommsCheck
 
; 
public

 
class

 
CommsCheckOptions

 
(

 
IServiceCollection

 1
services

2 :
)

: ;
{ 
public 

byte 
[ 
] 
ShaKey 
{ 
get 
; 
set  #
;# $
}% &
=' (
new) ,
byte- 1
[1 2
$num2 4
]4 5
;5 6
public 

CommsCheckOptions 
AddJsonConfig *
(* +
)+ ,
{ 
services 
. $
ConfigureHttpJsonOptions )
() *
options* 1
=>2 4
{ 	
options 
. 
SerializerOptions %
.% &
WriteIndented& 3
=4 5
true6 :
;: ;
options 
. 
SerializerOptions %
.% &
IncludeFields& 3
=4 5
true6 :
;: ;
options 
. 
SerializerOptions %
.% &

Converters& 0
.0 1
Add1 4
(4 5
new5 8#
JsonStringEnumConverter9 P
(P Q
)Q R
)R S
;S T
} 	
)	 

;
 
services 
. 
	Configure 
< 
JsonOptions &
>& '
(' (
options( /
=>0 2
{ 	
options 
. !
JsonSerializerOptions )
.) *

Converters* 4
.4 5
Add5 8
(8 9
new9 <#
JsonStringEnumConverter= T
(T U
)U V
)V W
;W X
} 	
)	 

;
 
return 
this 
; 
} 
public!! 

CommsCheckOptions!! 
AddDistriubtedCache!! 0
(!!0 1
)!!1 2
{"" 
services## 
.## %
AddDistributedMemoryCache## *
(##* +
)##+ ,
;##, -
return$$ 
this$$ 
;$$ 
}%% 
public'' 

CommsCheckOptions'' 

AddMetrics'' '
(''' (
)''( )
{(( 
services)) 
.)) 
AddOpenTelemetry)) !
())! "
)))" #
.++ 	
WithTracing++	 
(++ 
builder++ 
=>++ 
builder++  '
.,, (
AddAspNetCoreInstrumentation,, )
(,,) *
),,* +
)-- 
... 	
WithMetrics..	 
(.. 
builder// 
=>// 
builder// 
.00 
AddMeter00 
(00 
$str00 C
)00C D
.11 
AddMeter11 
(11 
$str11 @
)11@ A
.22 
AddMeter22 
(22 
$str22 ?
)22? @
.33 
AddMeter33 
(33 
$str33 F
)33F G
.44 
AddMeter44 
(44 
$str44 1
)441 2
.55 !
AddPrometheusExporter55 "
(55" #
)55# $
.66 (
AddAspNetCoreInstrumentation66 )
(66) *
)66* +
)77 
;77 
return99 
this99 
;99 
}:: 
public<< 

CommsCheckOptions<< 
AddRulesEngineRules<< 0
(<<0 1
IConfiguration== 
config== 
,== 
Action>> 
<>> 5
)CommsCheckRulesEngineConfigurationOptions>> 8
>>>8 9
options>>: A
)>>A B
{?? 
return@@ 
this@@ 
.@@ 
AddRulesEngineRules@@ '
<@@' (!
CommsCheckRulesEngine@@( =
>@@= >
(@@> ?
config@@? E
,@@E F
options@@G N
)@@N O
;@@O P
}AA 
publicCC 

CommsCheckOptionsCC 
AddRulesEngineRulesCC 0
<CC0 1
TCC1 2
>CC2 3
(CC3 4
IConfigurationDD 
configDD 
,DD 
ActionEE 
<EE 5
)CommsCheckRulesEngineConfigurationOptionsEE 8
>EE8 9
optionsEE: A
)EEA B
whereEEC H
TEEI J
:EEK L
classEEM R
,EER S

ICommCheckEET ^
{FF 
servicesGG 
.GG 
AddTransientGG 
<GG 

ICommCheckGG (
,GG( )
TGG* +
>GG+ ,
(GG, -
)GG- .
;GG. /
varHH 
optionsInstanceHH 
=HH 
newHH !5
)CommsCheckRulesEngineConfigurationOptionsHH" K
(HHK L
servicesHHL T
)HHT U
;HHU V
configKK 
.KK 

GetSectionKK 
(KK 5
)CommsCheckRulesEngineConfigurationOptionsKK C
.KKC D
OptionsNameKKD O
)KKO P
.KKP Q
BindKKQ U
(KKU V
optionsInstanceKKV e
)KKe f
;KKf g
optionsMM 
(MM 
optionsInstanceMM 
)MM  
;MM  !
servicesOO 
.OO 
	ConfigureOO 
<OO (
CommsCheckRulesEngineOptionsOO 7
>OO7 8
(OO8 9
xOO9 :
=>OO; =
{PP 	
xQQ 
.QQ 
JsonPathQQ 
=QQ 
optionsInstanceQQ (
.QQ( )
	RulesPathQQ) 2
;QQ2 3
}RR 	
)RR	 

;RR
 
returnTT 
thisTT 
;TT 
}UU 
publicWW 

CommsCheckOptionsWW 
	AddShaKeyWW &
(WW& '
stringWW' -
keyWW. 1
)WW1 2
{XX 
ShaKeyYY 
=YY 
EncodingYY 
.YY 
UTF8YY 
.YY 
GetBytesYY '
(YY' (
keyYY( +
)YY+ ,
;YY, -
services[[ 
.[[ 
	Configure[[ 
<[[ 
HashWrapperOptions[[ -
>[[- .
([[. /
x[[/ 0
=>[[1 3
{\\ 	
x]] 
.]] 
HashKey]] 
=]] 
ShaKey]] 
;]] 
}^^ 	
)^^	 

;^^
 
services`` 
.`` 
TryAddSingleton``  
<``  !'
HashWrapperObjectPoolPolicy``! <
>``< =
(``= >
)``> ?
;``? @
servicesaa 
.aa 
TryAddSingletonaa  
<aa  !
ObjectPoolProvideraa! 3
,aa3 4%
DefaultObjectPoolProvideraa5 N
>aaN O
(aaO P
)aaP Q
;aaQ R
servicesbb 
.bb 
TryAddSingletonbb  
<bb  !

ObjectPoolbb! +
<bb+ ,
HashWrapperbb, 7
>bb7 8
>bb8 9
(bb9 :
serviceProviderbb: I
=>bbJ L
{cc 	
vardd 
providerdd 
=dd 
serviceProviderdd *
.dd* +
GetRequiredServicedd+ =
<dd= >
ObjectPoolProviderdd> P
>ddP Q
(ddQ R
)ddR S
;ddS T
varee 
policyee 
=ee 
serviceProvideree (
.ee( )
GetRequiredServiceee) ;
<ee; <'
HashWrapperObjectPoolPolicyee< W
>eeW X
(eeX Y
)eeY Z
;eeZ [
returnff 
providerff 
.ff 
Createff "
(ff" #
policyff# )
)ff) *
;ff* +
}gg 	
)gg	 

;gg
 
returnii 
thisii 
;ii 
}jj 
}kk Â
h/home/rb/source/rossbuggins-public/commcheck-api/Extensions/CommsCheckRulesEngineConfigurationOptions.cs
	namespace 	

CommsCheck
 
; 
public 
class 5
)CommsCheckRulesEngineConfigurationOptions 6
(6 7
IServiceCollection7 I
servicesJ R
)R S
{ 
public 

const 
string 
OptionsName $
=% &
$str' 3
;3 4
public 

string 
	RulesPath 
{ 
get 
;  
set  #
;# $
}$ %
=& '
string( .
.. /
Empty/ 4
;4 5
public 
5
)CommsCheckRulesEngineConfigurationOptions 4
AddContactType5 C
<C D
TD E
,E F
UG H
>H I
(I J
)J K
where 
T 
: 
class 
, 
IContactType $
where		 
U		 
:		 
class		 
,		 )
ICommsCheckRulesEngineRuleRun		 5
<		5 6
T		6 7
>		7 8
{

 
services 
. 
AddTransient 
( 
typeof 
( )
ICommsCheckRulesEngineRuleRun 0
<0 1
IContactType1 =
>= >
)> ?
,? @
typeof 
( 
U 
) 
) 
; 
return 
this 
; 
} 
public 
5
)CommsCheckRulesEngineConfigurationOptions 4
AddContactType5 C
<C D
TD E
>E F
(F G
)G H
whereI N
TO P
:P Q
IContactTypeR ^
{ 
services 
. 
AddTransient 
( 
typeof $
($ %)
ICommsCheckRulesEngineRuleRun% B
<B C
IContactTypeC O
>O P
)P Q
,Q R
typeofS Y
(Y Z.
"CommsCheckRulesEngineRuleRunEventsZ |
<| }
T} ~
>~ 
)	 Ä
)
Ä Å
;
Å Ç
return 
this 
; 
} 
} ä
J/home/rb/source/rossbuggins-public/commcheck-api/Extensions/HashWrapper.cs
	namespace 	

CommsCheck
 
; 
public 
class 
HashWrapper 
{ 
private 
readonly 

HMACSHA256 
alg  #
;# $
private 
readonly 
ILogger 
< 
HashWrapper (
>( )
_logger* 1
;1 2
public

 
HashWrapper

 
(

 
ILogger

 !
<

! "
HashWrapper

" -
>

- .
logger

/ 5
,

5 6
IOptions

7 ?
<

? @
HashWrapperOptions

@ R
>

R S
options

T [
)

[ \
{ 
_logger 
= 
logger 
; 
var 
key 
= 
options 
. 
Value 
.  
HashKey  '
;' (
var 
b 
= 
new 
byte 
[ 
key 
. 
Length #
]# $
;$ %
Array 
. 
Copy 
( 
key 
, 
b 
, 
key 
. 
Length %
)% &
;& '
b 	
[	 

$num
 
] 
= 
( 
byte 
) 
$num 
; 
alg 
= 
new 

HMACSHA256 
( 
b 
) 
;  
} 
public 

async 
Task 
< 
string 
> 
GetSha $
($ %
CommsCheckItem% 3
item4 8
,8 9
string: @
reasonForHashA N
)N O
{ 
try 
{ 	
var 
str 
= 
item 
. 
ToString #
(# $
)$ %
.% &
Trim& *
(* +
)+ ,
., -
ToUpper- 4
(4 5
)5 6
;6 7
var 
b 
= 
System 
. 
Text 
.  
Encoding  (
.( )
UTF8) -
.- .
GetBytes. 6
(6 7
str7 :
): ;
;; <
var 
hash 
= 
alg 
. 
ComputeHash &
(& '
b' (
)( )
;) *
var 
hashStr 
= 
BitConverter &
.& '
ToString' /
(/ 0
hash0 4
)4 5
.5 6
Replace6 =
(= >
$str> A
,A B
$strC E
)E F
.F G
ToLowerG N
(N O
)O P
;P Q
_logger 
. 
LogInformation "
(" #
$str# U
,U V
reasonForHashW d
,d e
hashStrf m
,m n
stro r
)r s
;s t
return 
hashStr 
; 
} 	
catch   
(   
	Exception   
ex   
)   
{!! 	
_logger"" 
."" 
LogError"" 
("" 
ex"" 
,""  
$str""! 6
)""6 7
;""7 8
throw## 
;## 
}$$ 	
}%% 
}'' ¶
Z/home/rb/source/rossbuggins-public/commcheck-api/Extensions/HashWrapperObjectPoolPolicy.cs
	namespace 	

CommsCheck
 
; 
public 
class '
HashWrapperObjectPoolPolicy (
(( )
IServiceProvider) 9
sp: <
)< =
: 
PooledObjectPolicy 
< 
HashWrapper  
>  !
{ 
private 
static 
readonly 
Meter !
MyMeter" )
=* +
new, /
(/ 0
$str0 ]
,] ^
$str_ d
)d e
;e f
private		 
static		 
readonly		 
Counter		 #
<		# $
long		$ (
>		( )
CreatedCounter		* 8
=		9 :
MyMeter		; B
.		B C
CreateCounter		C P
<		P Q
long		Q U
>		U V
(		V W
$str		W r
)		r s
;		s t
private

 
static

 
readonly

 
Counter

 #
<

# $
long

$ (
>

( )
ReturnedCounter

* 9
=

: ;
MyMeter

< C
.

C D
CreateCounter

D Q
<

Q R
long

R V
>

V W
(

W X
$str

X t
)

t u
;

u v
public 

override 
HashWrapper 
Create  &
(& '
)' (
{ 
CreatedCounter 
. 
Add 
( 
$num 
) 
; 
return 
ActivatorUtilities !
.! "&
GetServiceOrCreateInstance" <
<< =
HashWrapper= H
>H I
(I J
spJ L
)L M
;M N
} 
public 

override 
bool 
Return 
(  
HashWrapper  +
obj, /
)/ 0
{ 
ReturnedCounter 
. 
Add 
( 
$num 
) 
; 
return 
true 
; 
} 
} •
Q/home/rb/source/rossbuggins-public/commcheck-api/Extensions/HashWrapperOptions.cs
	namespace 	

CommsCheck
 
; 
public 
class 
HashWrapperOptions 
{ 
public 

byte 
[ 
] 
HashKey 
{ 
get 
; 
set !
;! "
}" #
} “
K/home/rb/source/rossbuggins-public/commcheck-api/FunctionalHelpers/Empty.cs
	namespace 	
FunctionalHelpers
 
{ 
public 

class 
Empty 
< 
T 
> 
: 
Maybe !
<! "
T" #
># $
{ 
public 
Empty 
( 
) 
{ 	
} 	
}		 
}

 Î$
Z/home/rb/source/rossbuggins-public/commcheck-api/FunctionalHelpers/ForkExtensionMethods.cs
	namespace 	
FunctionalHelpers
 
{ 
public 

static 
class  
ForkExtensionMethods ,
{ 
public 
static 
Maybe 
< 
TOutputType '
>' (
Maybe) .
<. /

TInputType/ 9
,9 :
TOutputType; F
>F G
(G H
this 
Identity 
< 

TInputType $
>$ %
@this& +
,+ ,
Func- 1
<1 2
Identity2 :
<: ;

TInputType; E
>E F
,F G
TOutputTypeH S
>S T
fU V
)V W
{ 	
var		 
result		 
=		 
f		 
(		 
@this		  
)		  !
;		! "
Maybe 
< 
TOutputType 
> 
maybe $
=% &
EqualityComparer' 7
<7 8
TOutputType8 C
>C D
.D E
DefaultE L
.L M
EqualsM S
(S T
resultT Z
,Z [
default\ c
)c d
?e f
new 
Empty 
< 
TOutputType %
>% &
(& '
)' (
:) *
new 
Full 
< 
TOutputType $
>$ %
(% &
result& ,
), -
;- .
return 
maybe 
; 
} 	
public 
static 
async 
Task  
<  !
Maybe! &
<& '
TOutputType' 2
>2 3
>3 4

MaybeAsync5 ?
<? @

TInputType@ J
,J K
TOutputTypeL W
>W X
(X Y
this 
Identity 
< 

TInputType  
>  !
@this" '
,' (
Func) -
<- .
Identity. 6
<6 7

TInputType7 A
>A B
,B C
TaskD H
<H I
TOutputTypeI T
>T U
>U V
fW X
)X Y
{ 	
var 
result 
= 
await 
f  
(  !
@this! &
)& '
;' (
Maybe 
< 
TOutputType 
> 
maybe $
=% &
EqualityComparer' 7
<7 8
TOutputType8 C
>C D
.D E
DefaultE L
.L M
EqualsM S
(S T
resultT Z
,Z [
default\ c
)c d
?e f
new 
Empty 
< 
TOutputType %
>% &
(& '
)' (
:) *
new 
Full 
< 
TOutputType $
>$ %
(% &
result& ,
), -
;- .
return 
maybe 
; 
} 	
public 
static 
TOutputType !
Fork" &
<& '

TInputType' 1
,1 2
TOutputType3 >
>> ?
(? @
this 
Maybe 
< 

TInputType 
> 
@this $
,$ %
Func& *
<* +

TInputType+ 5
,5 6
TOutputType7 B
>B C
fEmptyD J
,J K
FuncL P
<P Q

TInputTypeQ [
,[ \
TOutputType] h
>h i
fFullj o
)o p
{   	
var!! 
rVal!! 
=!! 
@this!! 
switch!! #
{"" 
Empty## 
<## 

TInputType##  
>##  !
empty##" '
=>##( *
fEmpty##+ 1
(##1 2
empty##2 7
.##7 8
Value##8 =
)##= >
,##> ?
Full$$ 
<$$ 

TInputType$$ 
>$$  
full$$! %
=>$$& (
fFull$$) .
($$. /
full$$/ 3
.$$3 4
Value$$4 9
)$$9 :
,$$: ;
_%% 
=>%% 
throw%% 
new%% '
ArgumentOutOfRangeException%% :
(%%: ;
)%%; <
}&& 
;&& 
return(( 
rVal(( 
;(( 
})) 	
}** 
}++ µ
J/home/rb/source/rossbuggins-public/commcheck-api/FunctionalHelpers/Full.cs
	namespace 	
FunctionalHelpers
 
{ 
public 

class 
Full 
< 
T 
> 
: 
Maybe  
<  !
T! "
>" #
{ 
public 
Full 
( 
T 
value 
) 
{ 	
Value 
= 
value 
; 
}		 	
}

 
} É
N/home/rb/source/rossbuggins-public/commcheck-api/FunctionalHelpers/Identity.cs
	namespace 	
FunctionalHelpers
 
{ 
public 

class 
Identity 
< 
T 
> 
{ 
public 
T 
Value 
{ 
get 
; 
} 
public 
Identity 
( 
T 
value 
)  
=>! #
Value$ )
=* +
value, 1
;1 2
public

 
static

 
implicit

 
operator

 '
T

( )
(

) *
Identity

* 2
<

2 3
T

3 4
>

4 5
@this

6 ;
)

; <
=>

= ?
@this

@ E
.

E F
Value

F K
;

K L
} 
} ”
^/home/rb/source/rossbuggins-public/commcheck-api/FunctionalHelpers/IdentityExtensionMethods.cs
	namespace 	
FunctionalHelpers
 
{ 
public 

static 
class $
IdentityExtensionMethods 0
{ 
public 
static 
Identity 
< 
T  
>  !

ToIdentity" ,
<, -
T- .
>. /
(/ 0
this0 4
T5 6
@this7 <
)< =
=>> @
newA D
IdentityE M
<M N
TN O
>O P
(P Q
@thisQ V
)V W
;W X
} 
} ˆ
K/home/rb/source/rossbuggins-public/commcheck-api/FunctionalHelpers/Maybe.cs
	namespace 	
FunctionalHelpers
 
{ 
public 

abstract 
class 
Maybe 
<  
T  !
>! "
{ 
public 
virtual 
T 
Value 
{  
get! $
;$ %
	protected& /
set0 3
;3 4
}5 6
} 
} «	
Z/home/rb/source/rossbuggins-public/commcheck-api/HostedServices/CommsCheckHostedService.cs
	namespace 	

CommsCheck
 
; 
public 
class #
CommsCheckHostedService $
($ %
ChannelReader 
<  
CommsCheckItemWithId &
>& '
_reader( /
,/ 0

IPublisher		 

_publisher		 
)

 
:

 
BackgroundService

 
{ 
	protected 
override 
async 
Task !
ExecuteAsync" .
(. /
CancellationToken/ @
stoppingTokenA N
)N O
{ 
await 
foreach 
( 
var 
item 
in  "
_reader# *
.* +
ReadAllAsync+ 7
(7 8
stoppingToken8 E
)E F
)F G
{ 	
await 

_publisher 
. 
Publish #
(# $
new$ '!
MaybeItemToCheckEvent( =
(= >
item> B
)B C
)C D
;D E
} 	
} 
} Á
V/home/rb/source/rossbuggins-public/commcheck-api/Mediatr/AllICommsCheckEventHandler.cs
	namespace 	

CommsCheck
 
; 
public 
class &
AllICommsCheckEventHandler '
<' (
T( )
>) *
:+ , 
INotificationHandler- A
<A B
TB C
>C D
where 	
T
 
: 
ICommsCheckEvent 
{ 
private 
readonly 
ILogger 
< &
AllICommsCheckEventHandler 7
<7 8
T8 9
>9 :
>: ;
_logger< C
;C D
public 
&
AllICommsCheckEventHandler %
(% &
ILogger& -
<- .&
AllICommsCheckEventHandler. H
<H I
TI J
>J K
>K L
loggerM S
)S T
{		 
_logger

 
=

 
logger

 
;

 
} 
public 

Task 
Handle 
( 
T 
notification %
,% &
CancellationToken' 8
cancellationToken9 J
)J K
{ 
_logger 
. 
LogInformation 
( 
$" !
$str! "
{" #
notification# /
./ 0
GetType0 7
(7 8
)8 9
.9 :
Name: >
}> ?
$str? B
{B C
notificationC O
}O P
$strP Q
"Q R
)R S
;S T
return 
Task 
. 
CompletedTask !
;! "
} 
} ú
S/home/rb/source/rossbuggins-public/commcheck-api/Mediatr/LoggingCommandsBehavior.cs
	namespace 	

CommsCheck
 
; 
public 
class #
LoggingCommandsBehavior $
<$ %
TRequest% -
,- .
	TResponse/ 8
>8 9
:: ;
IPipelineBehavior< M
<M N
TRequestN V
,V W
	TResponseX a
>a b
where 	
TRequest
 
: 
IRequest 
< 
	TResponse '
>' (
{ 
private 
readonly 
ILogger 
< #
LoggingCommandsBehavior 4
<4 5
TRequest5 =
,= >
	TResponse? H
>H I
>I J
_loggerK R
;R S
public		 
#
LoggingCommandsBehavior		 "
(		" #
ILogger		# *
<		* +#
LoggingCommandsBehavior		+ B
<		B C
TRequest		C K
,		K L
	TResponse		M V
>		V W
>		W X
logger		Y _
)		_ `
{

 
_logger 
= 
logger 
; 
} 
public 

async 
Task 
< 
	TResponse 
>  
Handle! '
(' (
TRequest( 0
request1 8
,8 9"
RequestHandlerDelegate: P
<P Q
	TResponseQ Z
>Z [
next\ `
,` a
CancellationTokenb s
cancellationToken	t Ö
)
Ö Ü
{ 
_logger 
. 
LogInformation 
( 
$" !
$str! *
{* +
typeof+ 1
(1 2
TRequest2 :
): ;
.; <
Name< @
}@ A
"A B
)B C
;C D
var 
response 
= 
await 
next !
(! "
)" #
;# $
_logger 
. 
LogInformation 
( 
$" !
$str! )
{) *
typeof* 0
(0 1
	TResponse1 :
): ;
.; <
Name< @
}@ A
"A B
)B C
;C D
return 
response 
; 
} 
} ◊I
X/home/rb/source/rossbuggins-public/commcheck-api/Mediatr/PublishWithMetricsAndLogging.cs
	namespace 	

CommsCheck
 
; 
public 
class (
PublishWithMetricsAndLogging )
:* +
Mediator, 4
{ 
private		 
static		 
readonly		 
Meter		 !
MyMeter		" )
=		* +
new		, /
(		/ 0
$str		0 K
,		K L
$str		M R
)		R S
;		S T
private

 
static

 
readonly

 
Counter

 #
<

# $
long

$ (
>

( )
HandledCounter

* 8
=

9 :
MyMeter

; B
.

B C
CreateCounter

C P
<

P Q
long

Q U
>

U V
(

V W
$str

W p
)

p q
;

q r
private 
static 
readonly 
	Histogram %
<% &
double& ,
>, -
ProcessTime. 9
=: ;
MyMeter< C
.C D
CreateHistogramD S
<S T
doubleT Z
>Z [
([ \
$str\ x
)x y
;y z
private 
static 
readonly 
UpDownCounter )
<) *
long* .
>. /
CurrentlyProcessing0 C
=D E
MyMeterF M
.M N
CreateUpDownCounterN a
<a b
longb f
>f g
(g h
$str	h Ä
)
Ä Å
;
Å Ç
ILogger 
< (
PublishWithMetricsAndLogging (
>( )
_logger* 1
;1 2
public 
(
PublishWithMetricsAndLogging '
(' (
ILogger 
< (
PublishWithMetricsAndLogging )
>) *
logger+ 1
,1 2
IServiceProvider 
serviceFactory $
)$ %
: 
base 
( 
serviceFactory 
) 
{ 
_logger 
= 
logger 
; 
} 
	protected 
override 
async 
Task !
PublishCore" -
(- .
IEnumerable 
< '
NotificationHandlerExecutor /
>/ 0
handlerExecutors1 A
,A B
INotification 
notification "
," #
CancellationToken 
cancellationToken +
)+ ,
{ 
await 
Publish 
( 
handlerExecutors &
,& '
notification( 4
,4 5
cancellationToken6 G
)G H
;H I
} 
public 

async 
Task 
Publish 
( 
IEnumerable   
<   '
NotificationHandlerExecutor   -
>  - .
handlerExecutors  / ?
,  ? @
INotification!! 
notification!!  
,!!  !
CancellationToken"" 
cancellationToken"" )
)"") *
{## 
var$$ 
executorsList$$ 
=$$ 
handlerExecutors$$ ,
.$$, -
ToList$$- 3
($$3 4
)$$4 5
;$$5 6
_logger%% 
.%% 
LogInformation%% 
(%% 
$"%% !
$str%%! >
{%%> ?
notification%%? K
.%%K L
GetType%%L S
(%%S T
)%%T U
.%%U V
Name%%V Z
}%%Z [
$str%%[ a
{%%a b
executorsList%%b o
.%%o p
Count%%p u
}%%u v
$str%%v x
{%%x y
notification	%%y Ö
}
%%Ö Ü
$str
%%Ü á
"
%%á à
)
%%à â
;
%%â ä
foreach&& 
(&& 
var&& 
handler&& 
in&& 
executorsList&&  -
)&&- .
{'' 	
var(( 
sw(( 
=(( 
	Stopwatch(( 
.(( 
StartNew(( '
(((' (
)((( )
;(() *
HandledCounter** 
.** 
Add** 
(** 
$num++ 
,++ 
new,, 
KeyValuePair,,  
<,,  !
string,,! '
,,,' (
object,,) /
>,,/ 0
(,,0 1
$str,,1 F
,,,F G
handler,,H O
.,,O P
HandlerInstance,,P _
.,,_ `
GetType,,` g
(,,g h
),,h i
.,,i j
Name,,j n
),,n o
,,,o p
new-- 
KeyValuePair--  
<--  !
string--! '
,--' (
object--) /
>--/ 0
(--0 1
$str--1 ?
,--? @
notification--A M
.--M N
GetType--N U
(--U V
)--V W
.--W X
Name--X \
)--\ ]
)--] ^
;--^ _
CurrentlyProcessing// 
.//  
Add//  #
(//# $
$num00 
,00 
new11 
KeyValuePair11  
<11  !
string11! '
,11' (
object11) /
>11/ 0
(110 1
$str111 F
,11F G
handler11H O
.11O P
HandlerInstance11P _
.11_ `
GetType11` g
(11g h
)11h i
.11i j
Name11j n
)11n o
,11o p
new22 
KeyValuePair22  
<22  !
string22! '
,22' (
object22) /
>22/ 0
(220 1
$str221 ?
,22? @
notification22A M
.22M N
GetType22N U
(22U V
)22V W
.22W X
Name22X \
)22\ ]
)22] ^
;22^ _
_logger44 
.44 
LogInformation44 "
(44" #
$"44# %
$str44% E
{44E F
notification44F R
.44R S
GetType44S Z
(44Z [
)44[ \
.44\ ]
Name44] a
}44a b
$str44b h
{44h i
handler44i p
.44p q
HandlerInstance	44q Ä
.
44Ä Å
GetType
44Å à
(
44à â
)
44â ä
.
44ä ã
Name
44ã è
}
44è ê
$str
44ê í
{
44í ì
notification
44ì ü
}
44ü †
$str
44† °
"
44° ¢
)
44¢ £
;
44£ §
try66 
{77 
await88 
handler88 
.88 
HandlerCallback88 -
(88- .
notification88. :
,88: ;
cancellationToken88< M
)88M N
.88N O
ConfigureAwait88O ]
(88] ^
false88^ c
)88c d
;88d e
}99 
finally:: 
{;; 
sw<< 
.<< 
Stop<< 
(<< 
)<< 
;<< 
_logger== 
.== 
LogInformation== &
(==& '
$"==' )
$str==) I
{==I J
notification==J V
.==V W
GetType==W ^
(==^ _
)==_ `
.==` a
Name==a e
}==e f
$str==f l
{==l m
handler==m t
.==t u
HandlerInstance	==u Ñ
.
==Ñ Ö
GetType
==Ö å
(
==å ç
)
==ç é
.
==é è
Name
==è ì
}
==ì î
$str
==î ñ
{
==ñ ó
notification
==ó £
}
==£ §
$str
==§ •
"
==• ¶
)
==¶ ß
;
==ß ®
ProcessTime?? 
.?? 
Record?? "
(??" #
sw??# %
.??% &
Elapsed??& -
.??- .
TotalSeconds??. :
,??: ;
new@@ 
KeyValuePair@@ $
<@@$ %
string@@% +
,@@+ ,
object@@- 3
>@@3 4
(@@4 5
$str@@5 J
,@@J K
handler@@L S
.@@S T
HandlerInstance@@T c
.@@c d
GetType@@d k
(@@k l
)@@l m
.@@m n
Name@@n r
)@@r s
,@@s t
newAA 
KeyValuePairAA $
<AA$ %
stringAA% +
,AA+ ,
objectAA- 3
>AA3 4
(AA4 5
$strAA5 C
,AAC D
notificationAAE Q
.AAQ R
GetTypeAAR Y
(AAY Z
)AAZ [
.AA[ \
NameAA\ `
)AA` a
)AAa b
;AAb c
CurrentlyProcessingCC #
.CC# $
AddCC$ '
(CC' (
-DD 
$numDD 
,DD 
newEE 
KeyValuePairEE $
<EE$ %
stringEE% +
,EE+ ,
objectEE- 3
>EE3 4
(EE4 5
$strEE5 J
,EEJ K
handlerEEL S
.EES T
HandlerInstanceEET c
.EEc d
GetTypeEEd k
(EEk l
)EEl m
.EEm n
NameEEn r
)EEr s
,EEs t
newFF 
KeyValuePairFF $
<FF$ %
stringFF% +
,FF+ ,
objectFF- 3
>FF3 4
(FF4 5
$strFF5 C
,FFC D
notificationFFE Q
.FFQ R
GetTypeFFR Y
(FFY Z
)FFZ [
.FF[ \
NameFF\ `
)FF` a
)FFa b
;FFb c
}GG 
}HH 	
}II 
}JJ ¶E
;/home/rb/source/rossbuggins-public/commcheck-api/Program.cs
var

 
builder

 
=

 
WebApplication

 
.

 
CreateBuilder

 *
(

* +
args

+ /
)

/ 0
;

0 1
builder 
. 
Services 
. 
AddSingleton 
<  
RulesCombinerService 2
>2 3
(3 4
)4 5
;5 6
builder 
. 
Services 
. 
AddSingleton 
< +
RuleRunMethodResultCacheService =
>= >
(> ?
)? @
;@ A
builder 
. 
Services 
. 
AddCommsCheck 
( 
options &
=>' )
{ 
options 
. 
AddJsonConfig 
( 
) 
. 
AddDistriubtedCache  
(  !
)! "
. 
	AddShaKey 
( 
$str -
)- .
. 

AddMetrics 
( 
) 
. 
AddRulesEngineRules  
(  !
builder 
. 
Configuration %
.% &

GetSection& 0
(0 1
$str1 =
)= >
,> ?
ruleEngineOptions !
=>" $
{ 
ruleEngineOptions %
. 
AddContactType '
<' (
Sms( +
>+ ,
(, -
)- .
. 
AddContactType '
<' (
Email( -
>- .
(. /
)/ 0
. 
AddContactType '
<' (
Postal( .
>. /
(/ 0
)0 1
. 
AddContactType '
<' (
App( +
>+ ,
(, -
)- .
; 
}   
)   
;   
}!! 
)"" 
;"" 
builder$$ 
.$$ 
Services$$ 
.$$ #
AddEndpointsApiExplorer$$ (
($$( )
)$$) *
;$$* +
builder%% 
.%% 
Services%% 
.%% 
AddSwaggerGen%% 
(%% 
options'' 
=>'' 
{(( 
options)) 
.)) 
MapType)) 
<)) 
DateOnly))  
>))  !
())! "
())" #
)))# $
=>))% '
new))( +
OpenApiSchema)), 9
())9 :
))): ;
{** 	
Type++ 
=++ 
$str++ 
,++ 
Format,, 
=,, 
$str,, 
}-- 	
)--	 

;--
 
}.. 
)// 
;// 
var11 
app11 
=11 	
builder11
 
.11 
Build11 
(11 
)11 
;11 
if22 
(22 
app22 
.22 
Environment22 
.22 
IsDevelopment22 !
(22! "
)22" #
)22# $
{33 
app44 
.44 

UseSwagger44 
(44 
)44 
;44 
app55 
.55 
UseSwaggerUI55 
(55 
)55 
;55 
}66 
app77 
.77 )
MapPrometheusScrapingEndpoint77 !
(77! "
)77" #
;77# $
app99 
.99 
MapPost99 
(99 
$str99 
,99 
async:: 	
Task::
 
<:: 
Results:: 
<:: 
AcceptedAtRoute;; 
<;; )
CommsCheckQuestionResponseDto;; 9
>;;9 :
,;;: ;
CreatedAtRoute<< 
<<< )
CommsCheckQuestionResponseDto<< 8
><<8 9
><<9 :
><<: ;
(<<< =
[== 	
FromBody==	 
]== (
CommsCheckQuestionRequestDto== /
request==0 7
,==7 8
[>> 	
FromServices>>	 
]>> 
IDistributedCache>> (
cache>>) .
,>>. /
[?? 	
FromServices??	 
]?? 
ISender?? 
sender?? %
)??% &
=>??' )
{@@ 	
varAA 
resultAA 
=AA 
awaitAA 
senderAA %
.AA% &
SendAA& *
(AA* +
newAA+ .
CheckCommsCommandAA/ @
(AA@ A
requestAAA H
)AAH I
)AAI J
;AAJ K
varCC 
	itemBytesCC 
=CC 
awaitCC !
cacheCC" '
.CC' (
GetAsyncCC( 0
(CC0 1
resultCC1 7
.CC7 8
ResultIdCC8 @
)CC@ A
;CCA B
ifDD 
(DD 
	itemBytesDD 
==DD 
nullDD 
)DD 
returnEE 
TypedResultsEE #
.EE# $
CreatedAtRouteEE$ 2
(EE2 3
resultFF 
,FF 
$strGG %
,GG% &
newHH 
{HH 
resultIdHH "
=HH# $
resultHH% +
.HH+ ,
ResultIdHH, 4
}HH5 6
)HH6 7
;HH7 8
returnJJ 
TypedResultsJJ 
.JJ  
AcceptedAtRouteJJ  /
(JJ/ 0
resultKK 
,KK 
$strLL %
,LL% &
newMM 
{MM 
resultIdMM "
=MM# $
resultMM% +
.MM+ ,
ResultIdMM, 4
}MM5 6
)MM6 7
;MM7 8
}NN 	
)OO 
.PP 
WithNamePP 	
(PP	 

$strPP
 
)PP 
.QQ 
WithOpenApiQQ 
(QQ 
)QQ 
;QQ 
appSS 
.SS 
MapGetSS 

(SS
 
$strSS %
,SS% &
asyncTT 	
TaskTT
 
<TT 
ResultsTT 
<TT 
OkTT 
<TT '
CommsCheckAnswerResponseDtoTT 5
>TT5 6
,TT6 7
NotFoundTT8 @
>TT@ A
>TTA B
(TTC D
stringUU 
resultIdUU 
,UU 
[VV 	
FromServicesVV	 
]VV 
IDistributedCacheVV (
cacheVV) .
)VV. /
=>VV0 2
{WW 	
varXX 
	itemBytesXX 
=XX 
awaitXX !
cacheXX" '
.XX' (
GetAsyncXX( 0
(XX0 1
resultIdXX1 9
)XX9 :
;XX: ;
ifYY 
(YY 
	itemBytesYY 
==YY 
nullYY !
)YY! "
returnZZ 
TypedResultsZZ #
.ZZ# $
NotFoundZZ$ ,
(ZZ, -
)ZZ- .
;ZZ. /
var\\ 
itemStr\\ 
=\\ 
JsonSerializer\\ (
.\\( )
Deserialize\\) 4
<\\4 5
CommsCheckAnswer\\5 E
>\\E F
(\\F G
	itemBytes\\G P
)\\P Q
;\\Q R
var]] 
itemDto]] 
=]] '
CommsCheckAnswerResponseDto]] 5
.]]5 6 
FromCommsCheckAnswer]]6 J
(]]J K
itemStr]]K R
)]]R S
;]]S T
return^^ 
TypedResults^^ 
.^^  
Ok^^  "
(^^" #
itemDto^^# *
)^^* +
;^^+ ,
}__ 	
)`` 
.aa 
WithNameaa 	
(aa	 

$straa
 
)aa 
.bb 
WithOpenApibb 
(bb 
)bb 
;bb 
appdd 
.dd 
MapGetdd 

(dd
 
$strdd 
,dd 
asyncee 	
(ee
 
[ff 	
FromServicesff	 
]ff 
IOptionsff 
<ff  (
CommsCheckRulesEngineOptionsff  <
>ff< =
optionsff> E
)ffE F
=>ffG I
{gg 
varhh 
strhh 
=hh 
awaithh 
Filehh 
.hh 
ReadAllTextAsynchh -
(hh- .
optionshh. 5
.hh5 6
Valuehh6 ;
.hh; <
JsonPathhh< D
)hhD E
;hhE F
varii 
rulesii 
=ii 
Systemii 
.ii 
Textii 
.ii  
Jsonii  $
.ii$ %
JsonSerializerii% 3
.ii3 4
Deserializeii4 ?
<ii? @
objectii@ F
>iiF G
(iiG H
striiH K
)iiK L
;iiL M
returnjj 
TypedResultsjj 
.jj 
Jsonjj  
(jj  !
rulesjj! &
)jj& '
;jj' (
}kk 
)kk 
;kk 
appzz 
.zz 
Runzz 
(zz 
)zz 	
;zz	 
