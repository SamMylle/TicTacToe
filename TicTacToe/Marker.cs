#nullable enable
using System;

namespace TicTacToe {
    public class Marker {
        // A marker is placed on the board by a user. This marker is nothing more than a character.
        private readonly char _mark;

        public Marker(char x) {
            this._mark = x;
        }
        
        public override bool Equals(object? obj) {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            return this.Equals((Marker) obj);
        }

        protected bool Equals(Marker other) {
            return _mark == other._mark;
        }

        public override int GetHashCode() {
            return _mark.GetHashCode();
        }

        public override string ToString() {
            return this._mark.ToString();
        }
    }
}