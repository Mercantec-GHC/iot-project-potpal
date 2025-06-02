window.renderSensorChart = function (data) {
  if (typeof Chart === "undefined") {
    console.error("❌ Chart.js is not loaded. Cannot render chart.");
    return;
  }

  const ctx = document.getElementById("sensorChart").getContext("2d");

  const labels = data.map((d) => new Date(d.timestamp).toLocaleString());
  const temperature = data.map((d) => d.temperature);
  const airHumidity = data.map((d) => d.airHumidity);
  const soilMoisture = data.map((d) => d.soilMoisture);
  const lightLevel = data.map((d) => d.lightLevel);

  new Chart(ctx, {
    type: "line",
    data: {
      labels: labels,
      datasets: [
        {
          label: "Temperature (°C)",
          data: temperature,
          borderColor: "red",
          fill: false,
        },
        {
          label: "Air Humidity (%)",
          data: airHumidity,
          borderColor: "blue",
          fill: false,
        },
        {
          label: "Soil Moisture (%)",
          data: soilMoisture,
          borderColor: "green",
          fill: false,
        },
        {
          label: "Light Level (lux)",
          data: lightLevel,
          borderColor: "orange",
          fill: false,
        },
      ],
    },
    options: {
      responsive: true,
      scales: {
        x: { title: { display: true, text: "Time" } },
        y: { title: { display: true, text: "Value" } },
      },
    },
  });
};
