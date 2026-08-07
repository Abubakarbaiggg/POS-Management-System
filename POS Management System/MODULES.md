                         CUSTOMER ARRIVES
                               ↓
                      Enter Mobile Number
                               ↓
                   Search Customers Table
                               ↓
                    ┌──────────────────┐
                    │ Customer Exists? │
                    └────────┬─────────┘
                         YES │ NO
                             │
              ┌──────────────┴──────────────┐
              ↓                             ↓
       Existing Customer              New Customer
       Name appears                   Enter Name
       Mobile appears                 Mobile already entered
              │                             │
              └──────────────┬──────────────┘
                             ↓
                       CUSTOMER READY
                             ↓
                      ┌───────────────┐
                      │  ADD PRODUCTS │
                      └───────┬───────┘
                              ↓
                     Search / Scan Product
                              ↓
                      Product Found?
                              ↓
                         Add to Cart
                              ↓
                       Enter Quantity
                              ↓
                    Check Available Stock
                              ↓
                    ┌───────────────────┐
                    │ Stock Available?  │
                    └─────────┬─────────┘
                         YES  │  NO
                              │
                    ┌─────────┴─────────┐
                    ↓                   ↓
              Add Product          Show Error
              to Cart              "Out of Stock"
                    │
                    ↓
              Continue Adding?
                    ↓
             ┌──────┴──────┐
            YES            NO
             ↓              ↓
       Search / Scan       CART
       Another Product       ↓
                         ┌───────────────────┐
                         │ CART CALCULATION  │
                         └─────────┬─────────┘
                                   ↓
                              Product Total
                                   ↓
                         Quantity × Unit Price
                                   ↓
                              Subtotal
                                   ↓
                              Discount
                                   ↓
                           Apply Discount
                                   ↓
                              Tax Rate
                                   ↓
                            Calculate Tax
                                   ↓
                             Grand Total
                                   ↓
                         ┌─────────────────┐
                         │ PAYMENT         │
                         └────────┬────────┘
                                  ↓
                       Select Payment Method
                                  ↓
                    Cash / Card / Bank / etc.
                                  ↓
                         Enter Paid Amount
                                  ↓
                    Calculate Remaining / Change
                                  ↓
                         Confirm Payment
                                  ↓
                         COMPLETE SALE
                                  ↓
                    ┌──────────────────────┐
                    │ DATABASE TRANSACTION │
                    └──────────┬───────────┘
                               ↓
                          Create StockOut
                               ↓
                     Create StockOutDetails
                               ↓
                     Create CustomerPayment
                               ↓
                       Decrease Product Stock
                               ↓
                         Save Customer
                         if New Customer
                               ↓
                        Generate Invoice
                               ↓
                    ┌──────────────────────┐
                    │    SALE COMPLETED    │
                    └──────────┬───────────┘
                               ↓
                   ┌────────────────────────┐
                   │ Invoice Options        │
                   └───────────┬────────────┘
                               ↓
                    Print / Download PDF
                               ↓
                  Send Invoice on WhatsApp?
                               ↓
                     ┌─────────┴─────────┐
                    YES                  NO
                     ↓                    ↓
              WhatsApp API             Finish
                     ↓
              Send Invoice
                     ↓
              Customer receives
              WhatsApp message
                     ↓
                  FINISHED









                         CUSTOMER ARRIVES
                               ↓
                      Customer has Product
                               ↓
                    Search Invoice / Sale
                               ↓
                      Enter Invoice Number
                               ↓
                    System finds Invoice?
                               ↓
                    ┌──────────────────────┐
                    │ Invoice Found?       │
                    └──────────┬───────────┘
                           YES  │  NO
                                │
                   ┌────────────┴────────────┐
                   ↓                         ↓
             Show Sale Details          Show Error
                   ↓                    "Invoice Not Found"
          ┌────────────────────┐
          │ ORIGINAL SALE      │
          │                    │
          │ Keyboard × 2       │
          │ Mouse × 1          │
          │ Total = Rs. 5000   │
          └──────────┬─────────┘
                     ↓
             Select Product
                     ↓
            Select Product to Return
                     ↓
             Enter Return Quantity
                     ↓
        Check Purchased Quantity
                     ↓
        Check Already Returned Quantity
                     ↓
             ┌──────────────────────┐
             │ Return Quantity Valid?│
             └──────────┬───────────┘
                   YES  │  NO
                        │
              ┌─────────┴─────────┐
              ↓                   ↓
        Continue Return       Show Error
              ↓              "Invalid Quantity"
       Calculate Return Amount
              ↓
        Quantity × Unit Price
              ↓
        Return Amount
              ↓
       Select Return Reason
              ↓
    ┌──────────────────────────┐
    │ Return Reason            │
    │                          │
    │ Defective Product        │
    │ Wrong Product            │
    │ Damaged                  │
    │ Customer Changed Mind    │
    │ Other                    │
    └────────────┬─────────────┘
                 ↓
          Confirm Return
                 ↓
    ┌──────────────────────────┐
    │    DATABASE TRANSACTION  │
    └────────────┬─────────────┘
                 ↓
          Create Return Record
                 ↓
       Create Return Details
                 ↓
        Increase Product Stock
                 ↓
       Adjust Customer Payment
                 ↓
          Calculate Refund
                 ↓
          Generate Return Receipt
                 ↓
    ┌──────────────────────────┐
    │    RETURN COMPLETED      │
    └────────────┬─────────────┘
                 ↓
        ┌─────────────────────┐
        │ Receipt Options      │
        └──────────┬──────────┘
                   ↓
          Print / Download PDF
                   ↓
        Send Receipt on WhatsApp?
                   ↓
          ┌────────┴────────┐
         YES                NO
          ↓                  ↓
    WhatsApp API          Finish
          ↓
    Send Return Receipt
          ↓
    Customer receives
    WhatsApp message
          ↓
        FINISHED





                     SUPPLIER
                        ↓
                 Create Purchase
                        ↓
                 Select Supplier
                        ↓
                 Search Products
                        ↓
                  Add Products
                        ↓
                 Enter Quantity
                        ↓
                Enter Purchase Price
                        ↓
                  Calculate Total
                        ↓
                Create Purchase Order
                        ↓
                 Send to Supplier
                        ↓
                Supplier Delivers
                        ↓
                Receive Stock
                        ↓
             Check Received Quantity
                        ↓
              ┌─────────────────────┐
              │ Everything Correct? │
              └──────────┬──────────┘
                     YES │ NO
                         │
              ┌──────────┴──────────┐
              ↓                     ↓
        Receive Products       Record Difference
              ↓                     ↓
        Increase Stock         Partial Receiving
              │                     │
              └──────────┬──────────┘
                         ↓
                  Purchase Completed
                         ↓
                 Calculate Supplier Bill
                         ↓
                 Enter Paid Amount
                         ↓
              Calculate Remaining Amount
                         ↓
                Save Supplier Payment
                         ↓
                  Purchase Invoice
                         ↓
              Print / Download Invoice
                         ↓
                       FINISHED


                         BUSINESS
                            ↓
                  Create Branch / Warehouse
                            ↓
             ┌──────────────┼──────────────┐
             ↓              ↓              ↓
         Main Shop      Branch 1       Warehouse
             ↓              ↓              ↓
           Stock          Stock          Stock
             │              │              │
             └──────────────┼──────────────┘
                            ↓
                     Assign User
                            ↓
                  User selects branch
                            ↓
                    View Branch Stock
                            ↓
                 ┌──────────┴──────────┐
                 ↓                     ↓
            Stock In               Stock Out
                 ↓                     ↓
          Branch Stock +         Branch Stock -
                            ↓
                     Need Stock Transfer?
                            ↓
                    ┌───────┴───────┐
                   YES              NO
                    ↓                ↓
             Select From Branch    Finish
                    ↓
             Select To Branch
                    ↓
             Select Products
                    ↓
             Enter Quantity
                    ↓
             Create Transfer
                    ↓
             Stock leaves Source
                    ↓
             Transfer In Transit
                    ↓
             Receive at Destination
                    ↓
             Stock enters Destination
                    ↓
             Transfer Completed
                    ↓
             Transfer History
                    ↓
                  FINISHED



                         NEED STOCK
                             ↓
                     Create Purchase Request
                             ↓
                     Select Supplier
                             ↓
                     Search / Select Products
                             ↓
                      Enter Quantities
                             ↓
                   Enter Expected Price
                             ↓
                     Calculate Total
                             ↓
                   Submit Purchase Request
                             ↓
                    ┌──────────────────┐
                    │ Manager Approval │
                    └────────┬─────────┘
                         YES │ NO
                             │
                  ┌──────────┴──────────┐
                  ↓                     ↓
               APPROVED              REJECTED
                  ↓                     ↓
           Create Purchase Order     Add Reason
                  ↓                     ↓
           Send to Supplier          FINISHED
                  ↓
          Supplier Confirms Order
                  ↓
            Supplier Delivers
                  ↓
             Receive Stock
                  ↓
        Check Ordered vs Received
                  ↓
             ┌───────────────┐
             │ Match?        │
             └───────┬───────┘
                YES  │  NO
                     │
             ┌───────┴────────┐
             ↓                ↓
       Receive All       Partial Receive
             ↓                ↓
             └───────┬────────┘
                     ↓
               Update Stock
                     ↓
              Supplier Invoice
                     ↓
              Record Supplier Bill
                     ↓
               Supplier Payment
                     ↓
             Paid / Partially Paid
                     ↓
             Purchase Completed
                     ↓
             Purchase History
                     ↓
                   FINISHED