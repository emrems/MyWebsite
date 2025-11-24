import { createApp } from 'vue';
import Notification from '@/components/Notification.vue';

const NotificationPlugin = {
  install(app) {
    // Create mount point if it doesn't exist
    if (!document.getElementById('notification-mount-point')) {
      const mountPoint = document.createElement('div');
      mountPoint.id = 'notification-mount-point';
      document.body.appendChild(mountPoint);
    }

    // Create a new app instance for notifications
    const notificationApp = createApp(Notification);
    
    // Mount the notification component
    const mountedNotification = notificationApp.mount('#notification-mount-point');
    
    // Add to global properties
    app.config.globalProperties.$notify = mountedNotification;
    app.provide('notify', mountedNotification);
  }
};

export default NotificationPlugin;