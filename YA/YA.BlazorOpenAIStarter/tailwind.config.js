/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./**/*.{razor,html,cshtml}'],
    theme: {
        extend: {},
    },
    plugins: [
        require('daisyui'),
    ],
    daisyui: {
        themes: ["light", "dark", "synthwave"],
    },
}