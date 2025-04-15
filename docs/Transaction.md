# Question

So my system logic idea is this: When the passenger books a ride successfully and makes digital payment (in this case PayPal), they shall be charged an amount of money, which will not be immediately transferred to the driver's account, but only when the ride is complete. During that time, the fare will be temporarily kept in the business' account, and it will also charge a small fee for service fee (like, passenger pays 10$ for a ride but driver only gets 9.5$, while 0.5$ belongs to SmartRide). I think of 2 approaches:
1. Capture the payment immediately and send it to the business paypal account. when the ride is complete, create another transaction from business to driver. IMO this is easier but less efficient because we have to make 2 payments (PayPal may charge twice right?)
2. Somehow only authorize the transaction only, and capture it once the ride is complete. But can we split the fare into 2 amount to 2 accounts (business and driver) in a single transaction, or we still need 2 payments?

And beside, I'd like to support refund as well.

---

# Answer

Your system logic is well thought out, and both approaches are valid. Here's a breakdown of the two approaches and their implications:

---

### **Approach 1: Immediate Capture and Transfer**
1. **Flow**:
   - Capture the payment immediately when the passenger books the ride.
   - The full fare is transferred to the business's PayPal account.
   - Once the ride is complete, create a second transaction to transfer the driver's share to their PayPal account.

2. **Pros**:
   - Simpler to implement since PayPal's APIs are designed for capturing payments and making payouts.
   - No need to manage authorization expiration (authorizations expire after 29 days).

3. **Cons**:
   - PayPal will charge fees for both transactions (passenger → business, business → driver).
   - Less efficient due to the need for two separate payments.

4. **Refund Support**:
   - Refunds are straightforward since the business holds the full fare in its account.

---

### **Approach 2: Authorization and Delayed Capture**
1. **Flow**:
   - Authorize the payment when the passenger books the ride.
   - Capture the payment only when the ride is complete.
   - Split the captured amount into two parts:
     - Business's share (service fee).
     - Driver's share (remaining fare).

2. **Pros**:
   - More efficient since only one payment is processed.
   - Reduces PayPal fees (only one transaction fee for the passenger's payment).

3. **Cons**:
   - PayPal does not natively support splitting a single captured payment into multiple accounts. You would still need a second transaction (business → driver) to transfer the driver's share.
   - Requires managing authorization expiration (capture must occur within 29 days).

4. **Refund Support**:
   - Refunds are possible as long as the payment is authorized or captured.

---

### **Recommendation**
- **Use Approach 2** if you want to minimize PayPal fees and are comfortable managing authorization expiration.
- **Use Approach 1** if simplicity and ease of implementation are more important than reducing fees.

---

### **Implementation Details**

#### **For Approach 2 (Authorization and Delayed Capture)**:
1. **Authorize Payment**:
   - Use the `CheckoutPaymentIntent.Authorize` intent when creating the order.
   - The full fare is authorized but not captured.

2. **Capture Payment**:
   - When the ride is complete, capture the payment.
   - Transfer the driver's share to their PayPal account using a second transaction (e.g., PayPal Payouts API).

3. **Refund**:
   - Refund the authorized or captured payment if the ride is canceled.

#### **Code Changes for Authorization and Capture**

##### c:\Users\xmas4\OneDrive - Swinburne University\BA-CS\SWE30003 - Software Architectures and Design\swe30003\src\SmartRide.Infrastructure\Services\Transaction\PayPalProcessor.cs

Update the `CreateTransactionAsync` method to authorize the payment instead of capturing it.

````csharp
// ...existing code...
    public async Task<string> CreateTransactionAsync(
        decimal amount,
        string currency,
        RideTypeEnum rideType,
        VehicleTypeEnum vehicleType,
        string pickupAddress,
        string destinationAddress
        )
    {
        var orderRequest = new OrderRequest
        {
            Intent = CheckoutPaymentIntent.Authorize, // Change to "Authorize"
            PurchaseUnits =
            [
                new PurchaseUnitRequest
                {
                    Amount = new AmountWithBreakdown
                    {
                        CurrencyCode = currency,
                        MValue = amount.ToString("D4")
                    },
                    Items =
                    [
                        new Item
                        {
                            Name = "SmartRide Online Ride-Sharing Service",
                            Description = $"{rideType} ride (by {vehicleType}) from {pickupAddress} to {destinationAddress}.",
                            UnitAmount = new Money
                            {
                                CurrencyCode = currency,
                                MValue = amount.ToString("D4")
                            },
                            Quantity = "1",
                            Category = ItemCategory.DigitalGoods
                        }
                    ]
                }
            ]
        };

        var response = await _client.OrdersController.CreateOrderAsync(new CreateOrderInput { Body = orderRequest });
        if (response.StatusCode == 200)
        {
            return response.Data.Id; // Return the order ID
        }

        throw new Exception($"Failed to create order: {response}");
    }

    public async Task<bool> CaptureTransactionAsync(string transactionId, decimal driverShare, string driverPayPalEmail)
    {
        var response = await _client.OrdersController.CaptureOrderAsync(new CaptureOrderInput { Id = transactionId });
        if (response.StatusCode == 200)
        {
            // Use PayPal Payouts API to transfer driver's share
            await TransferToDriverAsync(driverShare, driverPayPalEmail);
            return true;
        }

        throw new Exception($"Failed to capture order: {response}");
    }

    private async Task TransferToDriverAsync(decimal amount, string driverPayPalEmail)
    {
        // Implement PayPal Payouts API logic here to transfer the driver's share
        throw new NotImplementedException();
    }
// ...existing code...
````

---

### **Refund Implementation**
Refunds can be implemented using the `PaymentsController` or `OrdersController` to refund an authorized or captured payment. This logic can be added to the `RefundTransactionAsync` method.

---

### Final Thoughts
- Approach 2 is more efficient but still requires a second transaction for the driver's share.
- Approach 1 is simpler but incurs higher fees.
- Both approaches support refunds, but Approach 2 requires careful handling of authorization expiration.

Made changes.