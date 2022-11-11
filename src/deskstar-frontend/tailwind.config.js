/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./pages/**/*.{js,ts,jsx,tsx}",
    "./components/**/*.{js,ts,jsx,tsx}",
    "./app/**/*.{js,ts,jsx,tsx}"
  ],
  theme: {
    extend: {
      colors: {
        "deskstar-green": {
          dark: "#5FC36D",
          light: "#9ee0b0"
        }
      }
    },
  },
  plugins: [require("daisyui")],
}
