using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WAW.Models;
namespace WAW.Views {
    public class Message {
        [Key] public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; } 
        public SenderType SenderType { get; set; }
        // Foreign Keys
        [ForeignKey("Conversation")]
        public int ConversationId { get; set; }
        // Navigation Properties
        public Conversation Conversation { get; set; }
    }
} public enum SenderType { Customer = 0, Advertiser = 1}