# run application

By default available at

```
http://localhost:5004/swagger/index.html
```
## Check 1 - DEA Set

- DEA is set, so all should be blocked.


Try out the api, POST /check

```
{
  "dateOfBirth": "2023-11-10",
  "dateOfSmsUpdate": "2023-11-10",
  "rfR": "DEA"
}
```

This shoud give a location of 

``` 
http://localhost:5004/check/result/b8bb8f3188fc20cd8972c0ff2a8b855bdf08e4f6d5fc09af537102951f45cd76
```

which should show

```
{
  "resultId": "b8bb8f3188fc20cd8972c0ff2a8b855bdf08e4f6d5fc09af537102951f45cd76",
  "requestString": "CommsCheckItem { DateOfBirth = 11/10/2023, DateOfSmsUpdated = 11/10/2023, ReasonForRemoval = Death { Code = DEA }, DaysOld = 0, DaySinceSmsUpdate = 0, YearsOld = 0 }",
  "app": "Blocked",
  "email": "Blocked",
  "sms": "Blocked",
  "postal": "Blocked",
  "appReason": "Explicit Block 10: DEA Block",
  "emailReason": "Explicit Block 10: DEA Block",
  "smsReason": "Explicit Block 10: DEA Block",
  "postalReason": "Explicit Block 10: DEA Block"
}
```

## Check 2 - No Reason for Removal Set

- No exit code, so all allowed.

try /check api again with 

```
{
  "dateOfBirth": "2023-11-10",
  "dateOfSmsUpdate": "2023-11-10"
}
```

This should give location of

```
http://localhost:5004/check/result/3e79b07ec8d3e7adeb68b235e4776f25387c13876a24c9fad4efa2c8ce38ff7b 
```

Which should give data of:

```
{
  "resultId": "3e79b07ec8d3e7adeb68b235e4776f25387c13876a24c9fad4efa2c8ce38ff7b",
  "requestString": "CommsCheckItem { DateOfBirth = 11/10/2023, DateOfSmsUpdated = 11/10/2023, ReasonForRemoval = NoReasonForRemoval { Code =  }, DaysOld = 0, DaySinceSmsUpdate = 0, YearsOld = 0 }",
  "app": "Allowed",
  "email": "Allowed",
  "sms": "Allowed",
  "postal": "Allowed",
  "appReason": "10 App allowed when no RfR is set.",
  "emailReason": "10 Email allowed when no RfR is set.",
  "smsReason": "10 Sms allowed when no RfR is set.",
  "postalReason": "10 Postal allowed when no RfR is set."
}
```

## Check 3 - CGA Reason for Removal set

Trying a check with a status of CGA. For this, an example rule has been created:

- Allowing send to SMS when CGA code is set.

```
{
  "dateOfBirth": "2023-11-10",
  "dateOfSmsUpdate": "2023-11-10",
  "rfR": "CGA"
}
```

gives location of

```
http://localhost:5004/check/result/2b479a85444d04f525f6fb32d5349108ee31ddff77ac5af8da004a8935e75430
```

which should show results of

```
{
  "resultId": "2b479a85444d04f525f6fb32d5349108ee31ddff77ac5af8da004a8935e75430",
  "requestString": "CommsCheckItem { DateOfBirth = 11/10/2023, DateOfSmsUpdated = 11/10/2023, ReasonForRemoval = MovedAway { Code = CGA }, DaysOld = 0, DaySinceSmsUpdate = 0, YearsOld = 0 }",
  "app": "Blocked",
  "email": "Blocked",
  "sms": "Allowed",
  "postal": "Blocked",
  "appReason": "Default Block",
  "emailReason": "Default Block",
  "smsReason": "20 Sms allowed to be sent even when CGA RfR is set",
  "postalReason": "Default Block"
}
```

# View rules

Going to:

```
http://localhost:5004/rules
```

this shows all currently active rules. The pattern for decision is:

1. If any ExpiicitBlock-All rules succeed, this will BLOCK all comminucation.

1. If an ExplicitBlock for the given comms type succeeds, this will BLOCK this commm type.

1. If an Allow for the given comms type succeeds, this will ALLOW this comms type.

1. If no rules success (block or allows), then by default if will BLOCK that communication type.

Example rules are

```
[
  {
    "WorkflowName": "ExplicitBlock-All",
    "Rules": [
      {
        "RuleName": "Block all DEA.",
        "SuccessEvent": "Explicit Block 10: DEA Block",
        "ErrorMessage": "",
        "Enabled": true,
        "Expression": "input1.ReasonForRemoval.Code == \"DEA\""
      }
    ]
  },
  {
    "WorkflowName": "ExplicitBlock-App",
    "Rules": [
      {
        "RuleName": "Block all App ",
        "SuccessEvent": "Explicit App Block 10: Block All App",
        "ErrorMessage": "",
        "Enabled": false,
        "Expression": "true"
      }
    ]
  },
  {
    "WorkflowName": "ExplicitBlock-Email",
    "Rules": [
      {
        "RuleName": "Block all Email ",
        "SuccessEvent": "Explicit Email Block 10: Block All Email",
        "ErrorMessage": "",
        "Enabled": false,
        "Expression": "true"
      }
    ]
  },
  {
    "WorkflowName": "ExplicitBlock-Sms",
    "Rules": [
      {
        "RuleName": "Block all SMS ",
        "SuccessEvent": "Explicit SMS Block 10: Block All Sms",
        "ErrorMessage": "",
        "Enabled": false,
        "Expression": "true"
      }
    ]
  },
  {
    "WorkflowName": "ExplicitBlock-Postal",
    "Rules": [
      {
        "RuleName": "Block all Postal ",
        "SuccessEvent": "Explicit Postal Block 10: Block All Postal",
        "ErrorMessage": "",
        "Enabled": false,
        "Expression": "true"
      }
    ]
  },
  {
    "WorkflowName": "Allow-App",
    "Rules": [
      {
        "RuleName": "AllowApp-HasNoExitCode",
        "SuccessEvent": "10 App allowed when no RfR is set.",
        "ErrorMessage": "",
        "Expression": "input1.ReasonForRemoval.NotSet == true"
      }
    ]
  },
  {
    "WorkflowName": "Allow-Email",
    "Rules": [
      {
        "RuleName": "AllowEmail-HasNoExitCode",
        "SuccessEvent": "10 Email allowed when no RfR is set.",
        "ErrorMessage": "",
        "Expression": "input1.ReasonForRemoval.NotSet == true"
      }
    ]
  },
  {
    "WorkflowName": "Allow-Sms",
    "Rules": [
      {
        "RuleName": "AllowSms-HasNoExitCode",
        "SuccessEvent": "10 Sms allowed when no RfR is set.",
        "ErrorMessage": "",
        "Expression": "input1.ReasonForRemoval.NotSet == true"
      },
      {
        "RuleName": "AllowSms-WithCGA",
        "SuccessEvent": "20 Sms allowed to be sent even when CGA RfR is set",
        "ErrorMessage": "",
        "Expression": "input1.ReasonForRemoval.Code == \"CGA\""
      }
    ]
  },
  {
    "WorkflowName": "Allow-Postal",
    "Rules": [
      {
        "RuleName": "Allow-PostalHasNoExitCode",
        "SuccessEvent": "10 Postal allowed when no RfR is set.",
        "ErrorMessage": "",
        "Expression": "input1.ReasonForRemoval.NotSet == true"
      }
    ]
  }
]
```
