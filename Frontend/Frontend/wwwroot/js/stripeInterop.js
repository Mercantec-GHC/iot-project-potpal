console.log("stripeInterop.js executing");

let stripe;
let elements;
let card;
let clientSecret;

window.initializeStripeElements = function (secret, publishableKey) {
    console.log("Initializing Stripe with clientSecret:", secret);
    console.log("Using publishableKey:", publishableKey);

    clientSecret = secret;

    stripe = Stripe(publishableKey);
    elements = stripe.elements();

    // Customize style here
    const style = {
        base: {
            color: "#32325d",
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSmoothing: "antialiased",
            fontSize: "16px",
            "::placeholder": {
                color: "#aab7c4"
            },
            backgroundColor: "#ffffff",
            padding: "12px 14px",
        },
        invalid: {
            color: "#fa755a",
            iconColor: "#fa755a"
        },
        complete: {
            color: "#4CAF50"
        }
    };

    card = elements.create("card", { style: style });
    card.mount("#payment-element");

    console.log("Stripe card element mounted.");
};

window.submitStripePayment = async function () {
    console.log("Submitting payment...");
    if (!stripe || !card) {
        console.error("Stripe or card is not initialized.");
        return { success: false, message: "Stripe.js not ready" };
    }

    try {
        const result = await stripe.confirmCardPayment(clientSecret, {
            payment_method: {
                card: card
            }
        });

        if (result.error) {
            console.error("Stripe error:", result.error.message);
            return { success: false, message: result.error.message };
        } else if (result.paymentIntent?.status === "succeeded") {
            console.log("Payment succeeded.");
            return { success: true, message: "Payment succeeded" };
        } else {
            return { success: false, message: "Unexpected status: " + result.paymentIntent?.status };
        }
    } catch (err) {
        console.error("Exception in submitStripePayment:", err);
        return { success: false, message: err.message || "Unknown error" };
    }
};
