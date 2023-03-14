# template-js-vite-vanilla

# [Vite](https://vitejs.dev/)

`npm create vite@latest my-app --template vanilla`  
Verder de instructies in de commandline gevolgd.

Inspecteer `package.json`.

In deze template staat de `source code in my-app`. Configuratie bestanden
zijn daarbuiten gehouden. Dus verplaatst naar de root van het project.

Bestand toegevoegd
`vite.config.js`, een configuratie betand helpt Vite de source code te vinden:

```
export default {
    root: 'my-app'
}
```

# [Prettier](https://prettier.io/)

`npm install --save-dev --save-exact prettier`

Bestanden toegevoegd:
`.prettierignore`
`.prettierrc.cjs`

Test:
`npx prettier --write .`

Script in `package.json`:

```
"scripts": {
    ...
    "format": "prettier -w --list-different ."
    ...
  },
```

# [ESlint](https://eslint.org/)

`npm init @eslint/config -- --config semistandard`

Scripts in `package.json`:

```
  "scripts": {
    ...
    "lint": "eslint .",
    "lint:fix": "eslint --fix ."
    ...
  },
```

Test scripts in `package.json`. Fix de errors 😉.

# [lint-staged](https://github.com/okonet/lint-staged) en [Husky](https://github.com/typicode/husky)

`npm install --save-dev lint-staged`
`npm install husky -D`

Bestand toegevoegd:
`lintstagedrc.cjs`

Scripts in `package.json`

```
"scripts": {
    ...
    "lint:staged": "lint-staged",
    "prepare": ""cd .. && husky install .husky""
  },
```

Toevoegen van map `.husky`

Installeren
`npm run prepare`
`npx husky add .husky/pre-commit "npm run lint:staged"`

# [Cypress](https://go.cypress.io/)

`npm install cypress --save-dev`

Scripts in `package.json`

```
"scripts": {
  "e2e:verify": "cypress verify",
  "e2e:start:build": "npm run build && concurrently --raw --kill-others --success \"all\" \"npm run preview\" \"cypress open --e2e --config-file cypress.prod.ts\"",
  "e2e:start:dev": "concurrently --raw --kill-others --success \"all\" \"npm run dev\" \"cypress open --e2e\"",
  "e2e:run:build": "npm run build && concurrently --raw --kill-others --success \"all\" \"npm run preview\" \"cypress run --e2e --config-file cypress.prod.ts\"",
  "e2e:run:dev": "concurrently --raw --kill-others --success \"all\" \"npm run dev\" \"cypress run --e2e\"",
},
```

# [PostCSS](https://github.com/postcss) && [cssnano](https://cssnano.co/)

`npm i -D cssnano`
`npm i -D autoprefixer`

Bestand toegevoegd:
`postcss.config.cjs`
