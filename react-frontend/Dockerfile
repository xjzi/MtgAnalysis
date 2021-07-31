# Build environment
FROM node:16-alpine3.11 as build

# Create app directory
WORKDIR /app

# Install app dependencies
# A wildcard is used to ensure both package.json AND package-lock.json are copied
# where available (npm@5+)
COPY package*.json ./

# If you are building your code for production
RUN npm ci --only=production --audit=false

# Bundle app source
COPY . .

# Build to static files
RUN npm run build

# Production environment
FROM nginx:1.21.1-alpine

# Move build files to production
COPY --from=build /app/build /usr/share/nginx/html

# All paths should point to index.html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]