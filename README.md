# NewWalletAPITask
First Errand Pay Task

The Wallet API contains two controllers.

The first is the Authentication controller with endpoints for registration and sign in
Note: Password length must be at least 8 characters and it must contain 
-at least One upper case letter

-at least One lower case letter

-at least one special symbol(@,#,)

-at least one digit

On signing in, the User(Owner) receives a JWT(JSON Web Token) which it uses to be authorized to access the Wallet Controller

A User can create only one wallet. Default starting balance is 0.0

A User can view his wallet details.

A User can Fund their wallet (I integrated with Monnify API to enable transfer into the wallet)

A User can Transfer to a bank account(I integrated with Monnify API to enable transfer from the wallet to the virtual bank account)

A User can view details of all the transactions in and out of the wallet (for reporting later on)

For the FundWallet and Transfer endpoints, I tested them using the sample request body on monnify website, but I change the payment reference to avoid duplicate payment reference.
