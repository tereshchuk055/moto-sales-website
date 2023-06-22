module.exports = {
  content: [
    './Views/**/*.cshtml'
],
  darkMode: 'class', // or 'media' or false
  theme: {
      extend: {
          maxHeight: {
              'logo': '50px',
              'avatar' : '42px'
          },
          spacing: {
              "negative" : "-42px"
          },
          animation: {
              marquee: 'marquee 50s linear infinite',
              marquee2: 'marquee2 50s linear infinite',
          },
          keyframes: {
              marquee: {
                  '0%': { transform: 'translateX(0%)' },
                  '100%': { transform: 'translateX(-100%)' },
              },
              marquee2: {
                  '0%': { transform: 'translateX(100%)' },
                  '100%': { transform: 'translateX(0%)' },
              },
          },
      },
  },
  variants: {
    extend: {},
  },
  plugins: [],
}