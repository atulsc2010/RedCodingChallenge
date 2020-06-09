# RedCodingChallenge

![design diagram](/RedApiChallenge.jpg)

#How to Run 
Use the below hosted Api Gatewaend points 
```
	https://myservice.altocumulus.it/api/fibonacci?n=10
	https://myservice.altocumulus.it/api/reversewords?sentence=this%20is%20a%20sentence!
	https://myservice.altocumulus.it/api/triangletype?a=2&b=2&c=2

```

Running in Visual Studio Mock Lamda Test Tool
```
	Enter the following message on the Mock Lamda test tool UI Function Input box. Make sure Function and Region drop down are populated. 
	{
	  "resource": "/{proxy+}",
	  "path": "/api/fibonacci",
	  "httpMethod": "GET",
	  "queryStringParameters": {
		"n": "5"
	  }
	}

```