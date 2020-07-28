namespace TicTacToe {
    public class Field {
        // A Field is a tile on the board (which consequently consists of 9 Fields)
        // Each Field may contain a marker of a player (or none)
        private Marker _mark;

        public Field() {
            this._mark = null;
        }

        public bool IsEmpty() {
            // Return whether or not a player has marked this Field
            return this._mark == null;
        }

        public void Mark(Marker marker) {
            // Put this player's marker on the Field
            this._mark = marker;
        }

        public bool Marked(Marker marker) {
            // Return whether or not this field has been marked by the given marker
            return this._mark != null && marker.Equals(this._mark);
        }

        public override string ToString() {
            // For printing the Field in the context of the entire board
            if (this._mark == null)
                return "|   ";
            else
                return "| " + this._mark + " ";
        }
    }
}