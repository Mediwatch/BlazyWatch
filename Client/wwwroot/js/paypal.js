function init_paypal() {
    if (document.getElementById("paypal-button-container") != null){
        paypal.Buttons({
            createOrder: function (data, actions) {
                return actions.order.create({
                    purchase_units: [{
                        amount: {
                            value: '0.01'
                        }
                    }]
                });
            },
            onApprove: function (data, actions) {
                return actions.order.capture().then(function (details) {
                    alert('Transaction completed by ' + details.payer.name.given_name);
                });
            }
        }).render('#paypal-button-container'); // Display payment options on your web page
    }
}
//  <!--paypal PART -->
//  <script src="https://www.paypal.com/sdk/js?client-id=AbQlXImq--DsH3y2FbHYJBJsFzn4n2jCnME-voHlS_Out_ZMnLtt-hnG_XGvKXSkcWqBmO0ArtlGqjDI"> // Replace YOUR_SB_CLIENT_ID with your sandbox client ID
//  </script>
//  <div id="paypal-button-container"></div>

//  <!-- Add the checkout buttons, set up the order and approve the order -->
//  <script> 
//    paypal.Buttons({
//      createOrder: function(data, actions) {
//        return actions.order.create({
//          purchase_units: [{
//            amount: {
//              value: '0.01'
//            }
//          }]
//        });
//      },
//      onApprove: function(data, actions) {
//        return actions.order.capture().then(function(details) {
//          alert('Transaction completed by ' + details.payer.name.given_name);
//        });
//      }
//    }).render('#paypal-button-container'); // Display payment options on your web page
//  </script>

