FROM nginx:stable-alpine
LABEL version="1.0"

COPY nginx.conf /etc/nginx/nginx.conf

WORKDIR /usr/share/nginx/html
COPY ./ .
EXPOSE 80
CMD ["nginx","-g","daemon off;"]
