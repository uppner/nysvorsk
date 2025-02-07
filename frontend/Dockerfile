FROM node:22.11 as build

WORKDIR /build

COPY package.json .
COPY package-lock.json .

RUN npm ci

COPY . ./

RUN npm run build

FROM node:22.11-slim

WORKDIR /app

COPY --from=build /build/.output ./

ENV HOST=0.0.0.0
ENV NODE_ENV=production

EXPOSE 3000

CMD ["node", "/app/server/index.mjs"]

