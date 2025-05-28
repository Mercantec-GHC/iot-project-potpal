let stripe, elements, card;

window.initializeStripeElements = function (clientSecret, publishableKey) {
    window.clientSecret = clientSecret;

    stripe = Stripe(publishableKey);

    elements = stripe.elements();
    card = elements.create("card");
    card.mount("#payment-element");
};

window.submitStripePayment = async function () {
    const result = await stripe.confirmCardPayment(window.clientSecret, {
        payment_method: {
            card: card
        }
    });

    return result;
};