FROM nginx:1.17.9-alpine AS base
COPY WebUI/dist/WebUI /usr/share/nginx/html
COPY WebUI/nginx /etc/nginx
ENTRYPOINT ["/etc/nginx/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]

