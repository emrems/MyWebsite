<template>
  <div class="notification-container">
    <transition-group name="notification">
      <div 
        v-for="notification in notifications" 
        :key="notification.id"
        :class="['notification', `notification-${notification.type}`]"
        @click="removeNotification(notification.id)"
      >
        <div class="notification-icon">
          <i :class="getIcon(notification.type)"></i>
        </div>
        <div class="notification-content">
          <div class="notification-title">{{ notification.title }}</div>
          <div class="notification-message">{{ notification.message }}</div>
        </div>
        <button class="notification-close" @click.stop="removeNotification(notification.id)">
          <i class="fas fa-times"></i>
        </button>
      </div>
    </transition-group>
  </div>
</template>

<script>
export default {
  name: 'Notification',
  data() {
    return {
      notifications: [],
      counter: 0
    };
  },
  methods: {
    show(options) {
      const id = this.counter++;
      const notification = {
        id,
        type: options.type || 'info',
        title: options.title,
        message: options.message,
        duration: options.duration || 5000
      };

      this.notifications.push(notification);

      if (notification.duration > 0) {
        setTimeout(() => {
          this.removeNotification(id);
        }, notification.duration);
      }

      return id;
    },

    removeNotification(id) {
      this.notifications = this.notifications.filter(n => n.id !== id);
    },

    getIcon(type) {
      const icons = {
        success: 'fas fa-check-circle',
        error: 'fas fa-exclamation-circle',
        warning: 'fas fa-exclamation-triangle',
        info: 'fas fa-info-circle'
      };
      return icons[type] || icons.info;
    }
  }
};
</script>

<style scoped>
.notification-container {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 9999;
  max-width: 400px;
  width: 100%;
}

.notification {
  display: flex;
  align-items: flex-start;
  background: white;
  border-radius: 12px;
  padding: 16px;
  margin-bottom: 12px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.15);
  border-left: 4px solid;
  cursor: pointer;
  transition: all 0.3s ease;
  transform: translateX(0);
}

.notification:hover {
  transform: translateY(-2px);
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.2);
}

.notification-success {
  border-left-color: #10b981;
  background: linear-gradient(135deg, #f0fdf4, #ffffff);
}

.notification-error {
  border-left-color: #ef4444;
  background: linear-gradient(135deg, #fef2f2, #ffffff);
}

.notification-warning {
  border-left-color: #f59e0b;
  background: linear-gradient(135deg, #fffbeb, #ffffff);
}

.notification-info {
  border-left-color: #3b82f6;
  background: linear-gradient(135deg, #eff6ff, #ffffff);
}

.notification-icon {
  margin-right: 12px;
  font-size: 1.5rem;
  flex-shrink: 0;
}

.notification-success .notification-icon {
  color: #10b981;
}

.notification-error .notification-icon {
  color: #ef4444;
}

.notification-warning .notification-icon {
  color: #f59e0b;
}

.notification-info .notification-icon {
  color: #3b82f6;
}

.notification-content {
  flex: 1;
  min-width: 0;
}

.notification-title {
  font-weight: 600;
  font-size: 0.95rem;
  margin-bottom: 4px;
  color: #1f2937;
}

.notification-message {
  font-size: 0.875rem;
  color: #6b7280;
  line-height: 1.4;
}

.notification-close {
  background: none;
  border: none;
  color: #9ca3af;
  cursor: pointer;
  padding: 4px;
  margin-left: 8px;
  border-radius: 4px;
  transition: all 0.2s ease;
  flex-shrink: 0;
}

.notification-close:hover {
  background: #f3f4f6;
  color: #374151;
}

/* Animations */
.notification-enter-active,
.notification-leave-active {
  transition: all 0.4s ease;
}

.notification-enter-from {
  opacity: 0;
  transform: translateX(100%);
}

.notification-leave-to {
  opacity: 0;
  transform: translateX(100%);
}

.notification-move {
  transition: transform 0.4s ease;
}

@media (max-width: 768px) {
  .notification-container {
    top: 10px;
    right: 10px;
    left: 10px;
    max-width: none;
  }
}
</style>