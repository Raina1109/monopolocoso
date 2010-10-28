class Soldier
  def talk ()
    puts "Aye Aye Sir!"
  end
end

class Peon
  def talk ()
    puts "war war war"
  end
end

class Knight
  def talk ()
    puts "your wish is my command"
  end
end

def reply (unit)
  unit.talk
end

reply Peon.new
reply Soldier.new
reply Knight.new
