/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./**/*.{razor,html,cshtml}', '../YA.VideoTranscriberApp.BlazorUI.Client/**/*.{razor,html,cshtml}'],
    theme: {
        extend: {
            animation: {
                buttonheartbeat: 'buttonheartbeat 2s infinite ease-in-out',
            },
            keyframes: {
                buttonheartbeat: {
                    '0%': {
                        'box-shadow': '0 0 0 0 theme("colors.yellow.500")',
                        transform: 'scale(1)',
                    },
                    '50%': {
                        'box-shadow': '0 0 0 7px theme("colors.yellow.500/0")',
                        transform: 'scale(1.05)',
                    },
                    '100%': {
                        'box-shadow': '0 0 0 0 theme("colors.yellow.500/0")',
                        transform: 'scale(1)',
                    },
                },
            },
        },
    },
    plugins: [
        require('daisyui'),
        require('@tailwindcss/forms'),
    ],
    daisyui: {
        themes: ["light", "dark", "synthwave","forest"],
    },
}